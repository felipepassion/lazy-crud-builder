using Lazy.Crud.Builder.Platform.Hashing;
using Lazy.Crud.Builder.Platform.Markers;
using Xunit;

namespace Lazy.Crud.Builder.Platform.Tests;

public class ContentHasherTests
{
    private const string Body = "namespace X\n{\n    public class A { }\n}\n";

    [Fact]
    public void CrlfAndLfBodiesHashIdentically()
    {
        var lf = ContentHasher.Compute(Body);
        var crlf = ContentHasher.Compute(Body.Replace("\n", "\r\n"));
        Assert.Equal(lf.Value, crlf.Value);
        Assert.Equal(HashScope.BodyLf, lf.Scope);
        Assert.Equal(64, lf.Value.Length);
    }

    [Fact]
    public void ByteOrderMarkDoesNotChangeTheHash()
    {
        var plain = ContentHasher.Compute(Body);
        var bom = ContentHasher.Compute("﻿" + Body);
        Assert.Equal(plain.Value, bom.Value);
    }

    [Fact]
    public void MarkerIsExcludedFromTheHash()
    {
        var markerA = GeneratedCodeMarker.Render(Fields("1.0.0"), CommentSyntax.CSharpLine);
        var markerB = GeneratedCodeMarker.Render(Fields("9.9.9"), CommentSyntax.CSharpLine);
        var a = ContentHasher.Compute(markerA + Body);
        var b = ContentHasher.Compute(markerB + Body);
        var bare = ContentHasher.ComputeBody(Body, true);
        Assert.Equal(a.Value, b.Value);
        Assert.Equal(bare.Value, a.Value);
        Assert.Equal(HashScope.BodyLfMarkerExcluded, a.Scope);
        Assert.Equal(ContentHasher.ComputeBody(Body, false).Value, a.Value);
    }

    [Fact]
    public void OneCharacterBodyChangeAltersTheHash()
    {
        var a = ContentHasher.Compute(Body);
        var b = ContentHasher.Compute(Body.Replace("class A", "class B"));
        Assert.NotEqual(a.Value, b.Value);
    }

    [Fact]
    public void UnmarkedFileHashesWholeBodyWithBodyLfScope()
    {
        var json = "{\r\n  \"a\": 1\r\n}\r\n";
        var h = ContentHasher.Compute(json);
        Assert.Equal(HashScope.BodyLf, h.Scope);
        Assert.Equal(ContentHasher.Compute(json.Replace("\r\n", "\n")).Value, h.Value);
    }

    [Fact]
    public void MarkerInBlockSyntaxIsExcludedToo()
    {
        foreach (var syntax in CommentSyntax.All)
        {
            var marker = GeneratedCodeMarker.Render(Fields("1.0.0"), syntax);
            var hashed = ContentHasher.Compute(marker + Body);
            Assert.Equal(ContentHasher.ComputeBody(Body, true).Value, hashed.Value);
            Assert.Equal(HashScope.BodyLfMarkerExcluded, hashed.Scope);
        }
    }

    private static MarkerFields Fields(string version) => new()
    {
        Aggregate = "Products", Layer = "Domain", TemplateId = "domain.command-handlers",
        Ownership = Ownership.GeneratorOwned, GeneratorVersion = version, TemplateVersion = version,
        SourceTemplate = "Domain/DomainCommandHandlers.tt",
    };
}
