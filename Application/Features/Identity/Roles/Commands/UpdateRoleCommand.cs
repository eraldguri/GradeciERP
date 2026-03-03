using Application.Wrappers;
using MediatR;

namespace Application.Features.Identity.Roles.Commands;

public class UpdateRoleCommand : IRequest<IResponseWrapper>
{
    public UpdateRoleRequest? UpdateRole {  get; set; }
}

public class UpdateRoleCommandHandler(IRoleService roleService) : IRequestHandler<UpdateRoleCommand, IResponseWrapper>
{
    private readonly IRoleService _roleService = roleService;

    public async Task<IResponseWrapper> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var updateRole = await _roleService.UpdateAsync(request.UpdateRole!);
        return await ResponseWrapper.SuccessAsync(message: $"Role '{updateRole}' updated successfully.");
    }
}
