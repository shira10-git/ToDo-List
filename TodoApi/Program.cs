var builder = WebApplication.CreateBuilder(args);

// הוספת CORS
builder.Services.AddCors(options =>
{

    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// הוספת שירותים ל-Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// שימוש ב-CORS
app.UseCors("AllowAllOrigins");

// הפעלת Swagger בשלב הריצה
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // מגייס את Swagger UI
    app.UseSwaggerUI(); // מציג את ממשק ה-UI של Swagger
}

// נתוני דוגמא (רשימת משימות)
var tasks = new List<TaskItem>
{
    new TaskItem { Id = 1, Name = "Buy groceries", IsComplete = false },
    new TaskItem { Id = 2, Name = "Clean the house", IsComplete = true }
};

// שליפת כל המשימות
app.MapGet("/tasks", () =>
{
    return Results.Ok(tasks);
});

// הוספת משימה חדשה
app.MapPost("/tasks", (TaskItem newTask) =>
{
    newTask.Id = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1; // קביעת מזהה חדש
    tasks.Add(newTask);
    return Results.Created($"/tasks/{newTask.Id}", newTask);
});

// עדכון משימה
app.MapPut("/tasks/{id}", (int id, TaskItem updatedTask) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task == null)
        return Results.NotFound($"Task with ID {id} not found.");

    task.Name = updatedTask.Name;
    task.IsComplete = updatedTask.IsComplete;

    return Results.Ok(task);
});

// מחיקת משימה
app.MapDelete("/tasks/{id}", (int id) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task == null)
        return Results.NotFound($"Task with ID {id} not found.");

    tasks.Remove(task);
    return Results.NoContent();
});

// Route ברירת מחדל
app.MapGet("/", () => "Hello World!");

// הפעלת האפליקציה
app.Run();

// הגדרת המודל של משימה
record TaskItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsComplete { get; set; }
}
