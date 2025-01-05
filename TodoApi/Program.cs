using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

// הוספת CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.WithOrigins("https://todo-list-client-4na8.onrender.com")  // כתובת הלקוח שלך
               .AllowAnyMethod()
               .AllowAnyHeader());
});

// הוספת DbContext לשירותים
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"), 
    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.40-mysql")));

// הוספת שירותים ל-Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// שימוש ב-CORS
app.UseCors("AllowAll");

// הפעלת Swagger בשלב הריצה
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger(); // מייצר את ה-Swagger JSON
    app.UseSwaggerUI(); // מציג את ממשק ה-UI של Swagger
}

// שליפת כל המשימות
app.MapGet("/tasks", async (ToDoDbContext dbContext) =>
{
    var tasks = await dbContext.Items.ToListAsync(); // שליפת כל הנתונים מ-items
    return Results.Ok(tasks);
});

// הוספת משימה חדשה
app.MapPost("/tasks", async (ToDoDbContext dbContext, TodoApi.Item newTask) =>
{
    newTask.Id = 0; // ID חדש ייווצר אוטומטית
    dbContext.Items.Add(newTask); // הוספת המשימה לטבלה items
    await dbContext.SaveChangesAsync(); // שמירת השינויים
    return Results.Created($"/tasks/{newTask.Id}", newTask);
});

// עדכון משימה
app.MapPut("/tasks/{id}", async (ToDoDbContext dbContext, int id, Item updatedTask) =>
{
    var task = await dbContext.Items.FindAsync(id); // מציאת המשימה בטבלה items
    if (task == null)
        return Results.NotFound($"Task with ID {id} not found.");

    task.Name = updatedTask.Name;
    task.IsComplete = updatedTask.IsComplete;

    await dbContext.SaveChangesAsync(); // שמירת השינויים
    return Results.Ok(task);
});

// מחיקת משימה
app.MapDelete("/tasks/{id}", async (ToDoDbContext dbContext, int id) =>
{
    var task = await dbContext.Items.FindAsync(id); // חיפוש המשימה ב-items
    if (task == null)
        return Results.NotFound($"Task with ID {id} not found.");

    dbContext.Items.Remove(task); // מחיקת המשימה
    await dbContext.SaveChangesAsync(); // שמירת השינויים
    return Results.NoContent();
});

// Route ברירת מחדל
app.MapGet("/", () => "TodoApi is running");

// הפעלת האפליקציה
app.Run();
