using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Lazy.Crud.Builder.Platform.Descriptor;
using Lazy.Crud.Builder.Platform.Diagnostics;
using Lazy.Crud.Builder.Platform.Hashing;
using Lazy.Crud.Builder.Platform.Manifests;
using Lazy.Crud.Builder.Platform.Markers;

namespace Lazy.Crud.Builder.Platform.Writer
{
    /// <summary>Everything the shared writer needs to write one generated file.</summary>
    public sealed class WriteRequest
    {
        /// <summary>Solution root (the directory of the .sln); manifests, run context and diagnostics live under it.</summary>
        public string SolutionRoot { get; set; }
        public string AggregateName { get; set; }
        /// <summary>Layer id, for example Domain or Tests.Domain.</summary>
        public string Layer { get; set; }
        /// <summary>Stable template id from the registry; null for an unregistered call site.</summary>
        public string TemplateId { get; set; }
        /// <summary>Explicit ownership; null means "derive from the legacy boolean".</summary>
        public Ownership? Ownership { get; set; }
        /// <summary>The historical recreateFileIfExists flag.</summary>
        public bool RecreateFileIfExists { get; set; } = true;
        /// <summary>Absolute output directory (created when absent).</summary>
        public string OutputDirectory { get; set; }
        public string OutputFileName { get; set; }
        /// <summary>The generated body, without marker.</summary>
        public string Content { get; set; }
        /// <summary>Template source file, named in the marker's customization directive.</summary>
        public string SourceTemplate { get; set; }
        public Encoding Encoding { get; set; } = new UTF8Encoding(true);
        /// <summary>
        /// The T4 host writes the file (the wrapper's own output, for example Migrations.generated.bat)
        /// after the template returns. The writer then decides, records and marks exactly as for a
        /// file it writes itself, but returns the text the template must leave in its generation
        /// environment (<see cref="WriteResult.EmittedText"/>) instead of touching the disk.
        /// </summary>
        public bool HostWrites { get; set; }
    }

    public enum WriteOutcome
    {
        Written = 0,
        SkippedExisting = 1,
        Adopted = 2,
        Protected = 3,
        SkippedByFlag = 4,
        Failed = 5,
        RefusedManual = 6,
    }

    public sealed class WriteResult
    {
        public WriteOutcome Outcome { get; set; }
        public string RelativePath { get; set; }
        public string AbsolutePath { get; set; }
        public FileState? State { get; set; }
        public Ownership Ownership { get; set; }
        public string TemplateId { get; set; }
        public ContentHash Hash { get; set; }
        public List<Diagnostic> Diagnostics { get; } = new List<Diagnostic>();
        /// <summary>Host-written mode only: the exact text the host must write (marker included, or the protected file's current content).</summary>
        public string EmittedText { get; set; }
        public bool Succeeded { get { return Outcome != WriteOutcome.Failed && Outcome != WriteOutcome.Protected; } }
    }

    /// <summary>
    /// The single shared writer behind every template call site. It resolves ownership, checks the
    /// frozen hash baseline before overwriting a GeneratorOwned file, adopts unverified-but-identical
    /// files, protects drifted and unverified files unless a resolution flag is set, emits marker v2,
    /// records the manifest v2 entry and never swallows a failure.
    /// </summary>
    public static class GenerationWriter
    {
        public static WriteResult Write(WriteRequest request, IDiagnosticSink externalSink)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var result = new WriteResult();
            var collector = new DiagnosticCollector();
            var fileSink = string.IsNullOrEmpty(request.SolutionRoot) || string.IsNullOrEmpty(request.AggregateName)
                ? null
                : DiagnosticsFile.For(request.SolutionRoot, request.AggregateName);
            var sink = new CompositeDiagnosticSink(collector, externalSink, fileSink);

            try
            {
                WriteCore(request, sink, result);
            }
            catch (Exception ex)
            {
                result.Outcome = WriteOutcome.Failed;
                sink.Report(new Diagnostic(DiagnosticCodes.FileWriteFailed, DiagnosticSeverity.Error, result.RelativePath ?? request.OutputFileName ?? string.Empty,
                    "Unexpected failure while writing the generated file: " + ex.GetType().Name + ": " + ex.Message + ". Fix the cause and rerun the generation."));
            }
            if (fileSink != null && fileSink.LastWriteError != null)
            {
                var memoryOnly = new CompositeDiagnosticSink(collector, externalSink);
                memoryOnly.Report(new Diagnostic(DiagnosticCodes.DiagnosticsFileUnwritable, DiagnosticSeverity.Warning, fileSink.Path,
                    "The machine-readable diagnostics file could not be written: " + fileSink.LastWriteError.GetType().Name + ": " + fileSink.LastWriteError.Message + ". Diagnostics were still reported to the host."));
            }
            if (request.HostWrites && result.EmittedText == null)
            {
                // Never let the host overwrite a file the writer refused to write.
                result.EmittedText = ExistingContentOr(result.AbsolutePath, request.Content ?? string.Empty);
            }
            result.Diagnostics.AddRange(collector.Items);
            return result;
        }

        private static void WriteCore(WriteRequest request, IDiagnosticSink sink, WriteResult result)
        {
            var absolutePath = Path.GetFullPath(Path.Combine(request.OutputDirectory, request.OutputFileName));
            result.AbsolutePath = absolutePath;
            var relativePath = SolutionRoot.TryMakeRelative(SolutionRoot.NormalizeFullPath(request.SolutionRoot), absolutePath)
                               ?? absolutePath.Replace('\\', '/');
            result.RelativePath = relativePath;
            result.TemplateId = request.TemplateId;

            // 1. Ownership: explicit value, else registry, else legacy boolean.
            var registryEntry = TemplateRegistry.Find(request.TemplateId);
            Ownership ownership;
            if (request.Ownership.HasValue) ownership = request.Ownership.Value;
            else if (registryEntry != null) ownership = registryEntry.Ownership;
            else ownership = OwnershipNames.FromLegacyRecreateFlag(request.RecreateFileIfExists);

            bool unregistered = registryEntry == null;
            if (unregistered)
            {
                sink.Report(new Diagnostic(DiagnosticCodes.UnregisteredTemplate, DiagnosticSeverity.Warning, relativePath,
                    "The template '" + (request.TemplateId ?? request.SourceTemplate ?? "<unknown>") + "' has no entry in the platform template registry; its output is treated as user-owned (created only when absent). Register the template in the descriptor registry.")
                    .With("templateId", request.TemplateId ?? string.Empty));
                if (ownership == Ownership.GeneratorOwned) ownership = Ownership.GeneratedOnce;
            }
            result.Ownership = ownership;

            if (ownership == Ownership.Manual)
            {
                result.Outcome = WriteOutcome.RefusedManual;
                if (request.HostWrites) result.EmittedText = ExistingContentOr(absolutePath, string.Empty);
                sink.Report(new Diagnostic(DiagnosticCodes.ManualPathRefused, DiagnosticSeverity.Information, relativePath,
                    "The path is classified Manual and is never written by the generator."));
                return;
            }

            var runContext = RunContext.Load(request.SolutionRoot, request.AggregateName);
            var settings = Settings.LazySettings.ForSolution(request.SolutionRoot);
            var markersEnabled = runContext.MarkersEnabled && settings.GeneratedCodeMarkers;
            var syntax = markersEnabled ? CommentSyntax.ForFileName(request.OutputFileName) : null;
            if (!settings.GeneratedCodeMarkers)
            {
                sink.Report(new Diagnostic(DiagnosticCodes.CapabilityDisabled, DiagnosticSeverity.Information, relativePath,
                    "The generated-code-markers capability is disabled by lazy.settings (GeneratedCodeMarkers=False); the file is written without a marker and hashed as a whole body."));
            }

            var body = request.Content ?? string.Empty;
            var newHash = ContentHasher.ComputeBody(body, syntax != null);
            result.Hash = newHash;

            ManifestStore.WithLock(request.SolutionRoot, request.AggregateName, () =>
            {
                var load = ManifestStore.Load(request.SolutionRoot, request.AggregateName, sink);
                if (load.Failed)
                {
                    result.Outcome = WriteOutcome.Failed;
                    return false;
                }
                var manifest = load.Manifest ?? new GenerationManifest { AggregateName = request.AggregateName };
                var entry = manifest.Find(relativePath);
                var exists = File.Exists(absolutePath);

                // 2. GeneratedOnce: write only when absent.
                if (ownership == Ownership.GeneratedOnce)
                {
                    if (exists)
                    {
                        result.Outcome = WriteOutcome.SkippedExisting;
                        if (request.HostWrites) result.EmittedText = ExistingContentOr(absolutePath, string.Empty);
                        sink.Report(new Diagnostic(DiagnosticCodes.GeneratedOnceSkipped, DiagnosticSeverity.Information, relativePath,
                            "The file already exists and is user-owned after scaffold (GeneratedOnce); it was left unchanged."));
                        if (entry == null || entry.OwnershipValue == null)
                        {
                            // Record the user-owned seed so tools can answer ownership queries; its hash is informational.
                            var current = ContentHasher.Compute(File.ReadAllText(absolutePath));
                            manifest.Upsert(NewEntry(request, relativePath, ownership, current));
                            SaveManifest(manifest, request, sink, result);
                        }
                        return true;
                    }
                    return WriteFile(request, sink, result, manifest, relativePath, absolutePath, ownership, syntax, body, newHash, null);
                }

                // 3. GeneratorOwned: verify before overwriting.
                var state = DriftInspector.Inspect(request.SolutionRoot, relativePath, entry);
                result.State = state.State;
                switch (state.State)
                {
                    case FileState.New:
                    case FileState.Missing:
                        return WriteFile(request, sink, result, manifest, relativePath, absolutePath, ownership, syntax, body, newHash, state);

                    case FileState.Verified:
                        if (state.MarkerProblem)
                        {
                            sink.Report(new Diagnostic(DiagnosticCodes.MarkerProblem, DiagnosticSeverity.Warning, relativePath,
                                "The body matches the recorded hash but the marker header is missing or malformed; the file is regenerated with a valid marker. This is not content drift."));
                        }
                        return WriteFile(request, sink, result, manifest, relativePath, absolutePath, ownership, syntax, body, newHash, state);

                    case FileState.Unverified:
                        if (ContentHasher.HashEquals(state.CurrentHash != null ? state.CurrentHash.Value : null, newHash.Value))
                        {
                            // Identical body: adopt the baseline without a destructive write.
                            result.Outcome = WriteOutcome.Adopted;
                            var adopted = NewEntry(request, relativePath, ownership, newHash);
                            manifest.Upsert(adopted);
                            sink.Report(new Diagnostic(DiagnosticCodes.UnverifiedAdopted, DiagnosticSeverity.Information, relativePath,
                                "No baseline hash was recorded but the regenerated content is identical to the file on disk; the hash was recorded as the baseline without rewriting the file.")
                                .With("templateId", request.TemplateId ?? string.Empty).With("hash", newHash.Value));
                            var needsMarker = syntax != null && (state.Marker == null || !state.Marker.IsV2);
                            if (needsMarker)
                            {
                                // The body is proven identical, so rewriting only changes the marker block.
                                return WriteFile(request, sink, result, manifest, relativePath, absolutePath, ownership, syntax, body, newHash, state, WriteOutcome.Adopted);
                            }
                            return SaveManifest(manifest, request, sink, result);
                        }
                        return Protect(request, sink, result, manifest, relativePath, absolutePath, ownership, syntax, body, newHash, state, false);

                    case FileState.Drifted:
                        return Protect(request, sink, result, manifest, relativePath, absolutePath, ownership, syntax, body, newHash, state, true);
                }
                return false;
            });
        }

        private static bool Protect(WriteRequest request, IDiagnosticSink sink, WriteResult result, GenerationManifest manifest,
            string relativePath, string absolutePath, Ownership ownership, CommentSyntax syntax, string body, ContentHash newHash, FileStateResult state, bool drifted)
        {
            var runContext = RunContext.Load(request.SolutionRoot, request.AggregateName);
            var flags = "Resolve with --force (overwrite with the regenerated content) or --skip-modified (keep the file and continue).";
            switch (runContext.Resolution)
            {
                case ResolutionMode.Force:
                    sink.Report(new Diagnostic(DiagnosticCodes.ForcedOverwrite, DiagnosticSeverity.Warning, relativePath,
                        (drifted ? "Drifted" : "Unverified") + " file overwritten because the force flag was supplied; the previous content is lost.")
                        .With("templateId", request.TemplateId ?? string.Empty)
                        .With("previousHash", state.CurrentHash != null ? state.CurrentHash.Value : string.Empty));
                    return WriteFile(request, sink, result, manifest, relativePath, absolutePath, ownership, syntax, body, newHash, state);
                case ResolutionMode.SkipModified:
                    result.Outcome = WriteOutcome.SkippedByFlag;
                    if (request.HostWrites) result.EmittedText = ExistingContentOr(absolutePath, string.Empty);
                    sink.Report(new Diagnostic(DiagnosticCodes.SkippedModified, DiagnosticSeverity.Warning, relativePath,
                        (drifted ? "Drifted" : "Unverified") + " file left untouched because the skip-modified flag was supplied; the regenerated content was not written.")
                        .With("templateId", request.TemplateId ?? string.Empty));
                    return true;
                default:
                    result.Outcome = WriteOutcome.Protected;
                    if (request.HostWrites) result.EmittedText = ExistingContentOr(absolutePath, string.Empty);
                    if (drifted)
                    {
                        sink.Report(new Diagnostic(DiagnosticCodes.Drifted, DiagnosticSeverity.Error, relativePath,
                            "The file was modified outside the generator (recorded hash " + Short(state.Entry.ContentHash) + ", current hash " + Short(state.CurrentHash.Value) + ") and was NOT overwritten. " + flags)
                            .With("templateId", request.TemplateId ?? string.Empty)
                            .With("firstDifference", FirstDifference(absolutePath, body))
                            .With("expectedHash", state.Entry.ContentHash ?? string.Empty)
                            .With("actualHash", state.CurrentHash.Value)
                            .With("regeneratedHash", newHash.Value)
                            .With("resolutions", "force,skip-modified"));
                    }
                    else
                    {
                        sink.Report(new Diagnostic(DiagnosticCodes.Unverified, DiagnosticSeverity.Error, relativePath,
                            "No trustworthy baseline hash exists for this file (for example after a v1 manifest migration) and its content differs from the regenerated content; the difference cannot be attributed to you or to a template change, so the file was NOT overwritten. " + flags)
                            .With("templateId", request.TemplateId ?? string.Empty)
                            .With("firstDifference", FirstDifference(absolutePath, body))
                            .With("actualHash", state.CurrentHash != null ? state.CurrentHash.Value : string.Empty)
                            .With("regeneratedHash", newHash.Value)
                            .With("resolutions", "force,skip-modified"));
                    }
                    return false;
            }
        }

        private static bool WriteFile(WriteRequest request, IDiagnosticSink sink, WriteResult result, GenerationManifest manifest,
            string relativePath, string absolutePath, Ownership ownership, CommentSyntax syntax, string body, ContentHash newHash, FileStateResult state,
            WriteOutcome outcome = WriteOutcome.Written)
        {
            try
            {
                var dir = Path.GetDirectoryName(absolutePath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                result.Outcome = WriteOutcome.Failed;
                sink.Report(new Diagnostic(DiagnosticCodes.DirectoryCreateFailed, DiagnosticSeverity.Error, relativePath,
                    "The output directory could not be created: " + ex.GetType().Name + ": " + ex.Message + ". Check the path and permissions."));
                return false;
            }

            var text = body;
            if (syntax != null)
            {
                var marker = GeneratedCodeMarker.Render(new MarkerFields
                {
                    Aggregate = request.AggregateName,
                    Layer = request.Layer,
                    TemplateId = request.TemplateId,
                    Ownership = ownership,
                    GeneratorVersion = PlatformInfo.Version,
                    TemplateVersion = PlatformInfo.Version,
                    SourceTemplate = request.SourceTemplate,
                }, syntax);
                text = marker + body;
            }

            if (request.HostWrites)
            {
                result.EmittedText = text;
                result.Outcome = outcome;
                manifest.Upsert(NewEntry(request, relativePath, ownership, newHash));
                return SaveManifest(manifest, request, sink, result);
            }
            try
            {
                File.WriteAllText(absolutePath, text, request.Encoding ?? new UTF8Encoding(true));
            }
            catch (Exception ex)
            {
                result.Outcome = WriteOutcome.Failed;
                sink.Report(new Diagnostic(DiagnosticCodes.FileWriteFailed, DiagnosticSeverity.Error, relativePath,
                    "The generated file could not be written: " + ex.GetType().Name + ": " + ex.Message + ". Close any program locking the file or fix the permissions, then rerun.")
                    .With("templateId", request.TemplateId ?? string.Empty));
                return false;
            }

            result.Outcome = outcome;
            manifest.Upsert(NewEntry(request, relativePath, ownership, newHash));
            return SaveManifest(manifest, request, sink, result);
        }

        private static bool SaveManifest(GenerationManifest manifest, WriteRequest request, IDiagnosticSink sink, WriteResult result)
        {
            if (!ManifestStore.Save(manifest, request.SolutionRoot, sink))
            {
                result.Outcome = WriteOutcome.Failed;
                return false;
            }
            return true;
        }

        private static ManifestEntry NewEntry(WriteRequest request, string relativePath, Ownership ownership, ContentHash hash)
        {
            return new ManifestEntry
            {
                RelativePath = relativePath,
                Layer = request.Layer ?? string.Empty,
                TemplateId = request.TemplateId,
                TemplateVersion = PlatformInfo.Version,
                Ownership = OwnershipNames.ToName(ownership),
                ContentHash = hash.Value,
                HashAlgorithm = hash.Algorithm,
                HashScope = hash.Scope,
            };
        }

        /// <summary>
        /// Host-written variant of <see cref="Write"/>: same ownership, drift, marker and manifest
        /// rules, but the file is written by the T4 host afterwards. The template must replace its
        /// generation environment with <see cref="WriteResult.EmittedText"/>.
        /// </summary>
        public static WriteResult PrepareHostOutput(WriteRequest request, IDiagnosticSink externalSink)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            request.HostWrites = true;
            return Write(request, externalSink);
        }

        private static string ExistingContentOr(string absolutePath, string fallback)
        {
            try
            {
                if (!string.IsNullOrEmpty(absolutePath) && File.Exists(absolutePath)) return File.ReadAllText(absolutePath);
            }
            catch (Exception)
            {
                // Unreadable: the fallback keeps the host from writing an empty file over it.
            }
            return fallback;
        }

        /// <summary>
        /// The first line where the file on disk and the regenerated content differ, as
        /// "line N: on disk ... | regenerated ...". A pair of hashes says that two files differ; this
        /// says where, which is what a person needs in order to choose between keeping the edit and
        /// regenerating. The marker block is stripped from the disk side, since it is not compared.
        /// </summary>
        internal static string FirstDifference(string absolutePath, string regenerated)
        {
            try
            {
                if (!File.Exists(absolutePath)) return "the file does not exist";
                var onDisk = File.ReadAllText(absolutePath);
                MarkerBlock block;
                if (GeneratedCodeMarker.TryFindMarkerBlock(onDisk, out block) && block != null)
                    onDisk = onDisk.Substring(block.BodyStart);
                var a = ContentHasher.NormalizeLineEndings(onDisk).Split('\n');
                var b = ContentHasher.NormalizeLineEndings(regenerated ?? string.Empty).Split('\n');
                var count = Math.Min(a.Length, b.Length);
                for (var i = 0; i < count; i++)
                {
                    if (string.Equals(a[i], b[i], StringComparison.Ordinal)) continue;
                    return "line " + (i + 1) + ": on disk " + Quoted(a[i]) + " | regenerated " + Quoted(b[i]);
                }
                if (a.Length == b.Length) return "the compared bodies are equal; only the marker block differs";
                return a.Length > b.Length
                    ? "the file on disk has " + (a.Length - b.Length) + " more line(s); the first the regenerated content lacks is line " + (count + 1) + ": " + Quoted(a[count])
                    : "the regenerated content has " + (b.Length - a.Length) + " more line(s); the first missing from the file is line " + (count + 1) + ": " + Quoted(b[count]);
            }
            catch (Exception ex)
            {
                return "the difference could not be located: " + ex.GetType().Name + ": " + ex.Message;
            }
        }

        private static string Quoted(string line)
        {
            var text = (line ?? string.Empty).Trim();
            if (text.Length > 120) text = text.Substring(0, 117) + "...";
            return "\"" + text + "\"";
        }

        private static string Short(string hash)
        {
            return string.IsNullOrEmpty(hash) ? "<none>" : (hash.Length > 12 ? hash.Substring(0, 12) : hash);
        }
    }
}
