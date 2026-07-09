using Application.Wrappers;
using MediatR;

namespace Application.Features.Identity.Roles.Commands;

public class CreateRoleCommand : IRequest<IResponseWrapper>
{
    public CreateRoleRequest? CreateRole {  get; set; }
}

public class CreateCommandHandler(IRoleService roleService) : IRequestHandler<CreateRoleCommand, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleService.CreateAsync(request.CreateRole!);
        return await ResponseWrapper<RoleResponse>.SuccessAsync(data: role, message: $"Role '{role.Name}' created successfully.");
    }
}
