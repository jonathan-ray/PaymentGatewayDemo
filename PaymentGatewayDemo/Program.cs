using PaymentGatewayDemo.Application.Services;
using PaymentGatewayDemo.Infrastructure.Filters;
using PaymentGatewayDemo.Infrastructure.Security;
using PaymentGatewayDemo.Infrastructure.Repositories;
using PaymentGatewayDemo.Infrastructure.Repositories.Transformers;
using PaymentGatewayDemo.Application.Factories;
using PaymentGatewayDemo.Domain.Models.AcquiringBank;
using PaymentGatewayDemo.Domain.Services;
using PaymentGatewayDemo.Domain.Transformers;
using PaymentGatewayDemo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAcquiringBankServiceFacadeFactory, AcquiringBankServiceFacadeFactory>();

builder.Services.AddSingleton<IPaymentService, PaymentService>();

// Annotation: Exchange these lines to introduce user authorization by API key:
builder.Services.AddSingleton<IUserAuthorizationService, NoUserAuthorizationService>();
//builder.Services.AddSingleton<IUserAuthorizationService, UserAuthorizationService>();

builder.Services.AddSingleton<IAcquiringBankPaymentProcessingService<ProcessPaymentRequestModel, ProcessPaymentResponseModel>, SimulatedAcquiringBankService>();

builder.Services.AddSingleton<IPaymentModelTransformer, PaymentModelTransformer>();
builder.Services.AddSingleton<IAcquiringBankModelTransformer<ProcessPaymentRequestModel, ProcessPaymentResponseModel>, SimulatedAcquiringBankModelTransformer>();

builder.Services.AddSingleton<IMerchantDetailsTransformer>(serviceProvider => 
    new MerchantDetailsTransformer(serviceProvider.GetRequiredService<IDataEncrypterFactory>().CreateEncrypter(builder.Configuration.GetSection("ApiKeyEncryptionKey").Value)));
builder.Services.AddSingleton<IPaymentTransactionTransformer>(serviceProvider =>
    new PaymentTransactionTransformer(serviceProvider.GetRequiredService<IDataEncrypterFactory>().CreateEncrypter(builder.Configuration.GetSection("CardDetailsEncryptionKey").Value)));
builder.Services.AddSingleton<IRepository>(_ => new GenericRepository(DatabaseSeeder.SeedValue));
builder.Services.AddSingleton<IMerchantRepository, MerchantRepository>();
builder.Services.AddSingleton<IPaymentRepository, PaymentRepository>();

builder.Services.AddSingleton<IDataEncrypterFactory, DataEncrypterFactory>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpExceptionFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();