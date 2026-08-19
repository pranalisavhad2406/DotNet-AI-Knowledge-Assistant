var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Health endpoint
app.MapGet("/", () => Results.Ok(new
{
    application = "DotNet AI Knowledge Assistant",
    status = "Running",
    version = "1.0"
}))
.WithName("GetApplicationStatus");

// Chat endpoint - Mock implementation for now
app.MapPost("/api/chat", (ChatRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Message))
    {
        return Results.BadRequest(new
        {
            error = "Message is required."
        });
    }

    var response = new ChatResponse(
        request.Message,
        $"AI integration is coming next. You asked: {request.Message}"
    );

    return Results.Ok(response);
})
.WithName("Chat");

app.Run();

record ChatRequest(string Message);

record ChatResponse(
    string Question,
    string Answer
);