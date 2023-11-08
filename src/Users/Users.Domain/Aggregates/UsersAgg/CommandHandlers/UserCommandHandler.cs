using AutoMapper;
using CrossCutting.Application.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using LazyCrud.Core.Application.DTO.Extensions;
using LazyCrud.Core.Domain.CrossCutting;
using LazyCrud.Users.Domain.Aggregates.UsersAgg.Entities;
using LazyCrud.Users.Domain.Aggregates.UsersAgg.Repositories;
using LazyCrud.Users.Identity;

namespace LazyCrud.Users.Domain.Aggregates.UsersAgg.CommandHandlers;

public partial class UserCommandHandler
{
    public async override Task<DomainResponse> OnBeforeUpdateAsync(User entity)
    {
        using var repo = this._serviceProvider.GetRequiredService<IUserProfileListRepository>();

        entity.Accesses = (await repo.FindAllAsync(x => x.UserId == entity.Id)).ToList();
        return await base.OnBeforeUpdateAsync(entity);
    }

    public override async Task<DomainResponse> OnUpdateAsync(User entity, User entityAfter)
    {
        //using var _emailSender = this._serviceProvider.GetRequiredService<IEmailSender>();
        //using var userManager = this._serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //await CheckSendPasswordByMail(userManager, entity, _emailSender, entity.Id.ToString());

        if(entity.SelectedAccess?.UserProfile != null)
            _userRepository.Dettach(entity.SelectedAccess.UserProfile);

        entity.Accesses = entityAfter.Accesses;
        foreach (var item in entity.Accesses)
        {
            foreach (var profile in item.UserProfiles)
            {
                _userRepository.Attach(profile);
            }
        }

        foreach (var item in entity.Accesses)
        {
            if (item.Id == 0)
            {
                  _userRepository.Added(item);
            }
        }

        return await base.OnUpdateAsync(entity, entityAfter);
    }

    public async override Task<DomainResponse> OnCreateAsync(User entity)
    {
        using var _emailSender = this._serviceProvider.GetRequiredService<IEmailSender>();
        using var userRepository = this._serviceProvider.GetRequiredService<IUserRepository>();

        var newUser = entity.ProjectedAs<ApplicationUser>();

        using var userManager = this._serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (entity.Id == 0)
        {
            entity.UserName = newUser.UserName = new Random().Next(1000000, 9999999).ToString();
            var identityUserCreationResult = await userManager.CreateAsync(newUser);

            if (!identityUserCreationResult.Succeeded)
                return DomainResponse.Error(identityUserCreationResult.Errors?.ToDictionary(x => x.Code ?? "00", x => x.Description)!);

            entity.Id = entity.Contact.Id = newUser.Id;

            entity.NeedSendOnboardingMail = true;
        }

        //await CheckSendPasswordByMail(userManager, entity, _emailSender, newUser.Id.ToString());
        return DomainResponse.Ok();
    }

    private async Task CheckSendPasswordByMail(UserManager<ApplicationUser> userManager, User entity, IEmailSender _emailSender, string userId)
    {
        if (entity.NeedSendOnboardingMail == true && !string.IsNullOrWhiteSpace(entity.Contact?.Email))
        {
            var newUser = await userManager.FindByIdAsync(userId);
            var token = await userManager.GeneratePasswordResetTokenAsync(await userManager.FindByIdAsync(entity.Id.ToString()));
            var pwd = Guid.NewGuid().ToString().Split('-').First();
            var resulPwdResult = await userManager.ResetPasswordAsync(newUser, token, pwd);
            await _emailSender.SendEmailAsync(entity.Contact.Email!, "Bem vindo à LazyCrud", $"Seu Login: {entity.UserName} \nSua senha: {pwd}");
            entity.NeedSendOnboardingMail = false;
        }
    }

    public async override Task<DomainResponse> OnDeleteAsync(User entity)
    {
        using var userManager = this._serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        using var _emailSender = this._serviceProvider.GetRequiredService<IEmailSender>();

        var appUser = await userManager.FindByIdAsync(entity.Id.ToString());
        if (appUser != null)
        {
            var deleteResult = await userManager.DeleteAsync(appUser);
            if (deleteResult.Succeeded)
                await _emailSender.SendEmailAsync(entity.Contact.Email!, "Conta deletada", "Sua conta foi deletada com sucesso.");
        }

        return DomainResponse.Ok();
    }
}
