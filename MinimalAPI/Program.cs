using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<YogaDb>(opt =>
{
    opt.UseInMemoryDatabase("Yoga");
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.MapGet("/poses", async (YogaDb db) =>
    await db.Poses.ToListAsync());

app.MapGet("/poses/beginner", async (YogaDb db) =>
    await db.Poses.Where(t => t.IsBeginnerFriendly).ToListAsync());

app.MapGet("/poses/{id}", async (int id, YogaDb db) =>
    await db.Poses.FindAsync(id)
        is Pose pose
            ? Results.Ok(pose)
            : Results.NotFound());

app.MapPost("/poses", async (Pose pose, YogaDb db) =>
{
    db.Poses.Add(pose);
    await db.SaveChangesAsync();

    return Results.Created($"/poses/{pose.Id}", pose);
});

app.MapPut("/poses/{id:int}",async (int id, Pose inputPose, YogaDb db) =>
{
    var pose = await db.Poses.FindAsync(id);

    if (pose is null) return Results.NotFound();

    pose.Name = inputPose.Name;
    pose.IsBeginnerFriendly = inputPose.IsBeginnerFriendly;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/poses/{id}", async (int id, YogaDb db) =>
{
    if (await db.Poses.FindAsync(id) is Pose pose)
    {
        db.Poses.Remove(pose);
        await db.SaveChangesAsync();
        return Results.Ok(pose);
    }

    return Results.NotFound();
});

app.Run();


class Pose
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? SanskritName { get; set; }
    public bool IsBeginnerFriendly { get; set; } = false;
}

class YogaDb : DbContext
{
    public YogaDb(DbContextOptions<YogaDb> options)
        : base(options) { }

    public DbSet<Pose> Poses=> Set<Pose>();
}