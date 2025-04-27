using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace RestAPIDemo1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            //�[�J�A��
            builder.Services.AddRazorPages();
            //�[�J��Ʈw�A��
            builder.Services.AddDbContext<LabDBContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            /*builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new 
                {
                    Title = "My API",
                    Version = "v1"
                });
            });*/

            var app = builder.Build();
            //�[�J��Ʈw�A��
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<LabDBContext>();
                dbContext.Database.EnsureCreated(); // �۰ʫإ߸�Ʈw
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            // �[�J�����n��
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            ///<summary>
            ///�d�ߩҦ��ϥΪ� (Read)
            ///</summary>
            app.MapGet("/api/user", async (LabDBContext context) =>
            {
                return await context.Users.ToListAsync();
            });
            ///<summary>
            /// �d�߳�@�ϥΪ� (Read)
            ///</summary>
            app.MapGet("/api/users/{id}", async (int id, LabDBContext context) =>
            {
                var user = await context.Users.FindAsync(id);
                return user is not null ? Results.Ok(user) : Results.NotFound();
            });
            ///<summary>
            /// �s�W�ϥΪ� (Create)
            ///</summary>
            app.MapPost("/api/users", async (UserInfo userInfo, LabDBContext context) =>
            {
                context.Users.Add(userInfo);
                await context.SaveChangesAsync();
                return Results.Created($"/api/users/{userInfo.ID}", userInfo);
            });
            ///<summary>
            /// �ק�ϥΪ� (Update)
            ///</summary>
            app.MapPut("/api/users/{id}", async (int id, UserInfo updatedUser, LabDBContext context) =>
            {
                var user = await context.Users.FindAsync(id);
                if (user is null)
                {
                    return Results.NotFound();
                }

                user.Name = updatedUser.Name;
                user.PeopleId = updatedUser.PeopleId;

                await context.SaveChangesAsync();
                return Results.NoContent();
            });
            ///<summary>
            /// �R���ϥΪ� (Delete)
            /// </summary>
            app.MapDelete("/api/users/{id}", async (int id, LabDBContext context) =>
            {
                var user = await context.Users.FindAsync(id);
                if (user is null)
                {
                    return Results.NotFound();
                }

                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return Results.NoContent();
            });
            var Context = new[]
            {
                "id", "name", "peopleId"
            };
            app.MapGet("/Hello World!", (HttpContext httpContext) =>
            {
                var name = httpContext.Request.Query["name"].ToString();
                var id = httpContext.Request.Query["id"].ToString();
                return Results.Ok($"Hello {name} {id}");
            })
                .WithName("HelloWorld")
                .WithOpenApi();
            /*var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();*/

            app.MapRazorPages();
            app.Run();
        }
    }
}
