internal static class Startup
{
    public static WebApplication CreateWebApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGenBearer();
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                builder.Configuration.GetSection(JwtOptions.SectionKey)
                    .Bind(options.TokenValidationParameters);
                var securityKey = builder.Configuration
                    .GetSection(JwtOptions.SectionKey)[nameof(JwtOptions.SecurityKey)]
                    ?? throw new InvalidOperationException("JwtOptions:SecurityKey no está configurado.");
                options.TokenValidationParameters.IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            });
        builder.Services.AddAuthorization();
        builder.Services.AddNorthWindSalesServices(
            dbOptions => builder.Configuration.GetSection(DBOptions.SectionKey).Bind(dbOptions),
            smtpOptions => builder.Configuration.GetSection(SmtpOptions.SectionKey).Bind(smtpOptions),
            membershipDBOptions => builder.Configuration.GetSection(MembershipDBOptions.SectionKey).Bind(membershipDBOptions),
            jwtOptions => builder.Configuration.GetSection(JwtOptions.SectionKey).Bind(jwtOptions));
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyMethod();
                policy.AllowAnyHeader();
                policy.AllowAnyOrigin();
            });
        });

        return builder.Build();
    }

    public static WebApplication ConfigureWebApplication(this WebApplication app)
    {
        app.UseExceptionHandler(_ => { });
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapNorthWindSalesEndpoints();
        return app;
    }
}
