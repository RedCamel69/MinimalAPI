using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data.Context;
using MinimalAPI.Data.DTO;
using MinimalAPI.Data.Models;

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
    await db.Poses.Select(x => new PoseDTO(x)).ToListAsync());

app.MapGet("/poses/beginner", async (YogaDb db) =>
    await db.Poses.Where(t => t.IsBeginnerFriendly)
             .Select(x => new PoseDTO(x)).ToListAsync());

app.MapGet("/poses/{id}", async (int id, YogaDb db) =>
    await db.Poses.FindAsync(id)
        is Pose pose
            ? Results.Ok(new PoseDTO(pose))
            : Results.NotFound());

app.MapPost("/poses", async (PoseDTO poseDTO, YogaDb db) =>
{

    var pose = new Pose
    {
        IsBeginnerFriendly = poseDTO.IsBeginnerFriendly,
        Name = poseDTO.Name,
        SanskritName = poseDTO.SanskritName
    };

    db.Poses.Add(pose);
    await db.SaveChangesAsync();

    return Results.Created($"/poses/{pose.Id}", new PoseDTO(pose));
});

app.MapPut("/poses/{id:int}",async (int id, PoseDTO inputPoseDTO, YogaDb db) =>
{
    var pose = await db.Poses.FindAsync(id);

    if (pose is null) return Results.NotFound();

    pose.Name = inputPoseDTO.Name;
    pose.SanskritName = inputPoseDTO.SanskritName;
    pose.IsBeginnerFriendly = inputPoseDTO.IsBeginnerFriendly;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/poses/{id}", async (int id, YogaDb db) =>
{
    if (await db.Poses.FindAsync(id) is Pose pose)
    {
        db.Poses.Remove(pose);
        await db.SaveChangesAsync();
        return Results.Ok(new PoseDTO(pose));
    }

    return Results.NotFound();
});

app.Run();




