var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapPost("/api/images", async (HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();
    var file = form.Files.FirstOrDefault();

    if (file == null || file.Length == 0)
    {
        return Results.BadRequest("Invalid file.");
    }

    var fileName = form["fileName"].FirstOrDefault();
    if (string.IsNullOrWhiteSpace(fileName))
        return Results.BadRequest("fileName is missing.");

    var myPath = string.Join("/", fileName.Split(@"/"));
    var savePath = Path.Combine(Directory.GetCurrentDirectory(), "images", myPath);

    Directory.CreateDirectory(Path.GetDirectoryName(savePath));

    using (var fileStream = new FileStream(savePath, FileMode.Create))
    {
        await file.CopyToAsync(fileStream);
    }

    return Results.Ok(new { FileName = fileName });
});

app.MapGet("/api/images/{fileName}", async (string fileName) =>
{
    try
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);

        if (File.Exists(filePath))
        {
            var extension = Path.GetExtension(filePath).ToLower();

            if (extension == ".svg")
            {
                var fileStream = File.OpenRead(filePath);
                return Results.File(fileStream, "image/svg+xml");
            }
            else
            {
                var image = await Image.LoadAsync(filePath);
                var memoryStream = new MemoryStream();

                await image.SaveAsync(memoryStream, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder());
                memoryStream.Position = 0;

                return Results.File(memoryStream, "image/png");
            }
        }
        return Results.NotFound("File not found.");
    }
    catch (Exception)
    {
        throw;
    }
});

app.MapPost("/api/files", async (HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();
    var file = form.Files.FirstOrDefault();

    if (file == null || file.Length == 0)
    {
        return Results.BadRequest("Invalid file.");
    }

    var fileName = form["fileName"].FirstOrDefault() ?? file.FileName;
    if (string.IsNullOrWhiteSpace(fileName))
        return Results.BadRequest("fileName is missing.");

    var myPath = string.Join("/", fileName.Split(@"/"));
    var savePath = Path.Combine(Directory.GetCurrentDirectory(), "files", myPath);

    Directory.CreateDirectory(Path.GetDirectoryName(savePath));

    using (var fileStream = new FileStream(savePath, FileMode.Create))
    {
        await file.CopyToAsync(fileStream);
    }

    return Results.Ok(new { FileName = fileName });
});

app.MapGet("/api/files/{fileName}", async (string fileName) =>
{
    try
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "files", fileName);

        if (File.Exists(filePath))
        {
            var fileStream = File.OpenRead(filePath);

            return Results.File(fileStream, "application/octet-stream", fileName);
        }
        return Results.NotFound("File not found.");
    }
    catch (Exception)
    {
        throw;
    }
});

app.MapGet("", async () =>
{
    return Results.Ok("Images Api com SVG corrigido.");
});

app.Run();
