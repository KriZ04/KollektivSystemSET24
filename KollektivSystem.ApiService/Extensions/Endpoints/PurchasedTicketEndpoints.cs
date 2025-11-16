namespace KollektivSystem.ApiService.Extensions.Endpoints;

public static class PurchasedTicketEndpoints
{
    public static IEndpointRouteBuilder MapPurchasedTicketEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/tickets").RequireAuthorization();

        group.MapPost("", HandleBuyTicket);
        group.MapPost("validate", HandleTicketValidation).RequireAuthorization("Staff");

        var me = group.MapGroup("me");
        me.MapGet("", HandleGetMe);
        me.MapGet("{id:guid}", HandleGetMeById);

        var admin = group.MapGroup("admin").RequireAuthorization("Staff");
        admin.MapPost("{id:guid}/revoke", HandleRevoke);
        admin.MapGet("{id:guid}", HandleGetById);

        return app;
    }
    internal static async Task<IResult> HandleBuyTicket()
    {
        throw new NotImplementedException();
    }
    internal static async Task<IResult> HandleTicketValidation()
    {
        throw new NotImplementedException();
    }
    internal static async Task<IResult> HandleGetMe()
    {
        throw new NotImplementedException();
    }
    internal static async Task<IResult> HandleGetMeById()
    {
        throw new NotImplementedException();
    }
    internal static async Task<IResult> HandleRevoke()
    {
        throw new NotImplementedException();
    }
    internal static async Task<IResult> HandleGetById()
    {
        throw new NotImplementedException();
    }
}
