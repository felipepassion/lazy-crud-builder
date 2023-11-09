using LazyCrudBuilder.CrossCutting.Infra.Log.Contexts;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Commands;
namespace LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.CommandModels
{
    using Queries.Models; 
    using LazyCrudBuilder.Users.Application.DTO.Aggregates.UsersAgg.Requests; 
    public partial class CreateUserProfileAccessCommand : BaseRequestableCommand<UserProfileAccessQueryModel, UserProfileAccessDTO>
    {
        public bool UpdateIfExists { get; set; }
        public CreateUserProfileAccessCommand(ILogRequestContext ctx, UserProfileAccessDTO data, bool updateIfExists = true, UserProfileAccessQueryModel query = null) 
            : base(ctx, query, data) { this.UpdateIfExists = updateIfExists; }
    }
    public partial class DeleteUserProfileAccessCommand : BaseDeletionCommand<UserProfileAccessQueryModel>
    {
        public DeleteUserProfileAccessCommand(ILogRequestContext ctx, UserProfileAccessQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
      public partial class DeleteRangeUserProfileAccessCommand : BaseDeletionCommand<UserProfileAccessQueryModel>
    {
        public DeleteRangeUserProfileAccessCommand(ILogRequestContext ctx, UserProfileAccessQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
    public partial class UpdateRangeUserProfileAccessCommand : BaseRequestableRangeCommand<UserProfileAccessQueryModel, UserProfileAccessDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateRangeUserProfileAccessCommand(ILogRequestContext ctx, Dictionary<UserProfileAccessQueryModel, UserProfileAccessDTO> query)
            : base(ctx, query) { }
        public UpdateRangeUserProfileAccessCommand(ILogRequestContext ctx, UserProfileAccessQueryModel query, UserProfileAccessDTO data)
            : base(ctx, new Dictionary<UserProfileAccessQueryModel, UserProfileAccessDTO> { { query, data } }) { }
    }
    
    public partial class UpdateUserProfileAccessCommand : BaseRequestableCommand<UserProfileAccessQueryModel, UserProfileAccessDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateUserProfileAccessCommand(ILogRequestContext ctx, UserProfileAccessQueryModel query, UserProfileAccessDTO data, bool createIfNotExists = true)
            : base(ctx, query, data) { this.CreateIfNotExists = createIfNotExists; }
    }
    public partial class ActiveUserProfileAccessCommand : UserProfileAccessSearchableCommand
    {
        public ActiveUserProfileAccessCommand(ILogRequestContext ctx, UserProfileAccessQueryModel query)
            : base(ctx, query) { }
    }
    public partial class DeactiveUserProfileAccessCommand : UserProfileAccessSearchableCommand
    {
        public DeactiveUserProfileAccessCommand(ILogRequestContext ctx, UserProfileAccessQueryModel query)
            : base(ctx, query) { }
    }
    public class UserProfileAccessSearchableCommand : BaseSearchableCommand<UserProfileAccessQueryModel> {
        public UserProfileAccessSearchableCommand(ILogRequestContext ctx, UserProfileAccessQueryModel query)
            : base(ctx, query) { }
    }

    public partial class CreateUserCurrentAccessSelectedCommand : BaseRequestableCommand<UserCurrentAccessSelectedQueryModel, UserCurrentAccessSelectedDTO>
    {
        public bool UpdateIfExists { get; set; }
        public CreateUserCurrentAccessSelectedCommand(ILogRequestContext ctx, UserCurrentAccessSelectedDTO data, bool updateIfExists = true, UserCurrentAccessSelectedQueryModel query = null) 
            : base(ctx, query, data) { this.UpdateIfExists = updateIfExists; }
    }
    public partial class DeleteUserCurrentAccessSelectedCommand : BaseDeletionCommand<UserCurrentAccessSelectedQueryModel>
    {
        public DeleteUserCurrentAccessSelectedCommand(ILogRequestContext ctx, UserCurrentAccessSelectedQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
      public partial class DeleteRangeUserCurrentAccessSelectedCommand : BaseDeletionCommand<UserCurrentAccessSelectedQueryModel>
    {
        public DeleteRangeUserCurrentAccessSelectedCommand(ILogRequestContext ctx, UserCurrentAccessSelectedQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
    public partial class UpdateRangeUserCurrentAccessSelectedCommand : BaseRequestableRangeCommand<UserCurrentAccessSelectedQueryModel, UserCurrentAccessSelectedDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateRangeUserCurrentAccessSelectedCommand(ILogRequestContext ctx, Dictionary<UserCurrentAccessSelectedQueryModel, UserCurrentAccessSelectedDTO> query)
            : base(ctx, query) { }
        public UpdateRangeUserCurrentAccessSelectedCommand(ILogRequestContext ctx, UserCurrentAccessSelectedQueryModel query, UserCurrentAccessSelectedDTO data)
            : base(ctx, new Dictionary<UserCurrentAccessSelectedQueryModel, UserCurrentAccessSelectedDTO> { { query, data } }) { }
    }
    
    public partial class UpdateUserCurrentAccessSelectedCommand : BaseRequestableCommand<UserCurrentAccessSelectedQueryModel, UserCurrentAccessSelectedDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateUserCurrentAccessSelectedCommand(ILogRequestContext ctx, UserCurrentAccessSelectedQueryModel query, UserCurrentAccessSelectedDTO data, bool createIfNotExists = true)
            : base(ctx, query, data) { this.CreateIfNotExists = createIfNotExists; }
    }
    public partial class ActiveUserCurrentAccessSelectedCommand : UserCurrentAccessSelectedSearchableCommand
    {
        public ActiveUserCurrentAccessSelectedCommand(ILogRequestContext ctx, UserCurrentAccessSelectedQueryModel query)
            : base(ctx, query) { }
    }
    public partial class DeactiveUserCurrentAccessSelectedCommand : UserCurrentAccessSelectedSearchableCommand
    {
        public DeactiveUserCurrentAccessSelectedCommand(ILogRequestContext ctx, UserCurrentAccessSelectedQueryModel query)
            : base(ctx, query) { }
    }
    public class UserCurrentAccessSelectedSearchableCommand : BaseSearchableCommand<UserCurrentAccessSelectedQueryModel> {
        public UserCurrentAccessSelectedSearchableCommand(ILogRequestContext ctx, UserCurrentAccessSelectedQueryModel query)
            : base(ctx, query) { }
    }

    public partial class CreateUserProfileListCommand : BaseRequestableCommand<UserProfileListQueryModel, UserProfileListDTO>
    {
        public bool UpdateIfExists { get; set; }
        public CreateUserProfileListCommand(ILogRequestContext ctx, UserProfileListDTO data, bool updateIfExists = true, UserProfileListQueryModel query = null) 
            : base(ctx, query, data) { this.UpdateIfExists = updateIfExists; }
    }
    public partial class DeleteUserProfileListCommand : BaseDeletionCommand<UserProfileListQueryModel>
    {
        public DeleteUserProfileListCommand(ILogRequestContext ctx, UserProfileListQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
      public partial class DeleteRangeUserProfileListCommand : BaseDeletionCommand<UserProfileListQueryModel>
    {
        public DeleteRangeUserProfileListCommand(ILogRequestContext ctx, UserProfileListQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
    public partial class UpdateRangeUserProfileListCommand : BaseRequestableRangeCommand<UserProfileListQueryModel, UserProfileListDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateRangeUserProfileListCommand(ILogRequestContext ctx, Dictionary<UserProfileListQueryModel, UserProfileListDTO> query)
            : base(ctx, query) { }
        public UpdateRangeUserProfileListCommand(ILogRequestContext ctx, UserProfileListQueryModel query, UserProfileListDTO data)
            : base(ctx, new Dictionary<UserProfileListQueryModel, UserProfileListDTO> { { query, data } }) { }
    }
    
    public partial class UpdateUserProfileListCommand : BaseRequestableCommand<UserProfileListQueryModel, UserProfileListDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateUserProfileListCommand(ILogRequestContext ctx, UserProfileListQueryModel query, UserProfileListDTO data, bool createIfNotExists = true)
            : base(ctx, query, data) { this.CreateIfNotExists = createIfNotExists; }
    }
    public partial class ActiveUserProfileListCommand : UserProfileListSearchableCommand
    {
        public ActiveUserProfileListCommand(ILogRequestContext ctx, UserProfileListQueryModel query)
            : base(ctx, query) { }
    }
    public partial class DeactiveUserProfileListCommand : UserProfileListSearchableCommand
    {
        public DeactiveUserProfileListCommand(ILogRequestContext ctx, UserProfileListQueryModel query)
            : base(ctx, query) { }
    }
    public class UserProfileListSearchableCommand : BaseSearchableCommand<UserProfileListQueryModel> {
        public UserProfileListSearchableCommand(ILogRequestContext ctx, UserProfileListQueryModel query)
            : base(ctx, query) { }
    }

    public partial class CreateUserProfileCommand : BaseRequestableCommand<UserProfileQueryModel, UserProfileDTO>
    {
        public bool UpdateIfExists { get; set; }
        public CreateUserProfileCommand(ILogRequestContext ctx, UserProfileDTO data, bool updateIfExists = true, UserProfileQueryModel query = null) 
            : base(ctx, query, data) { this.UpdateIfExists = updateIfExists; }
    }
    public partial class DeleteUserProfileCommand : BaseDeletionCommand<UserProfileQueryModel>
    {
        public DeleteUserProfileCommand(ILogRequestContext ctx, UserProfileQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
      public partial class DeleteRangeUserProfileCommand : BaseDeletionCommand<UserProfileQueryModel>
    {
        public DeleteRangeUserProfileCommand(ILogRequestContext ctx, UserProfileQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
    public partial class UpdateRangeUserProfileCommand : BaseRequestableRangeCommand<UserProfileQueryModel, UserProfileDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateRangeUserProfileCommand(ILogRequestContext ctx, Dictionary<UserProfileQueryModel, UserProfileDTO> query)
            : base(ctx, query) { }
        public UpdateRangeUserProfileCommand(ILogRequestContext ctx, UserProfileQueryModel query, UserProfileDTO data)
            : base(ctx, new Dictionary<UserProfileQueryModel, UserProfileDTO> { { query, data } }) { }
    }
    
    public partial class UpdateUserProfileCommand : BaseRequestableCommand<UserProfileQueryModel, UserProfileDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateUserProfileCommand(ILogRequestContext ctx, UserProfileQueryModel query, UserProfileDTO data, bool createIfNotExists = true)
            : base(ctx, query, data) { this.CreateIfNotExists = createIfNotExists; }
    }
    public partial class ActiveUserProfileCommand : UserProfileSearchableCommand
    {
        public ActiveUserProfileCommand(ILogRequestContext ctx, UserProfileQueryModel query)
            : base(ctx, query) { }
    }
    public partial class DeactiveUserProfileCommand : UserProfileSearchableCommand
    {
        public DeactiveUserProfileCommand(ILogRequestContext ctx, UserProfileQueryModel query)
            : base(ctx, query) { }
    }
    public class UserProfileSearchableCommand : BaseSearchableCommand<UserProfileQueryModel> {
        public UserProfileSearchableCommand(ILogRequestContext ctx, UserProfileQueryModel query)
            : base(ctx, query) { }
    }

    public partial class CreateUsersAggSettingsCommand : BaseRequestableCommand<UsersAggSettingsQueryModel, UsersAggSettingsDTO>
    {
        public bool UpdateIfExists { get; set; }
        public CreateUsersAggSettingsCommand(ILogRequestContext ctx, UsersAggSettingsDTO data, bool updateIfExists = true, UsersAggSettingsQueryModel query = null) 
            : base(ctx, query, data) { this.UpdateIfExists = updateIfExists; }
    }
    public partial class DeleteUsersAggSettingsCommand : BaseDeletionCommand<UsersAggSettingsQueryModel>
    {
        public DeleteUsersAggSettingsCommand(ILogRequestContext ctx, UsersAggSettingsQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
      public partial class DeleteRangeUsersAggSettingsCommand : BaseDeletionCommand<UsersAggSettingsQueryModel>
    {
        public DeleteRangeUsersAggSettingsCommand(ILogRequestContext ctx, UsersAggSettingsQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
    public partial class UpdateRangeUsersAggSettingsCommand : BaseRequestableRangeCommand<UsersAggSettingsQueryModel, UsersAggSettingsDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateRangeUsersAggSettingsCommand(ILogRequestContext ctx, Dictionary<UsersAggSettingsQueryModel, UsersAggSettingsDTO> query)
            : base(ctx, query) { }
        public UpdateRangeUsersAggSettingsCommand(ILogRequestContext ctx, UsersAggSettingsQueryModel query, UsersAggSettingsDTO data)
            : base(ctx, new Dictionary<UsersAggSettingsQueryModel, UsersAggSettingsDTO> { { query, data } }) { }
    }
    
    public partial class UpdateUsersAggSettingsCommand : BaseRequestableCommand<UsersAggSettingsQueryModel, UsersAggSettingsDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateUsersAggSettingsCommand(ILogRequestContext ctx, UsersAggSettingsQueryModel query, UsersAggSettingsDTO data, bool createIfNotExists = true)
            : base(ctx, query, data) { this.CreateIfNotExists = createIfNotExists; }
    }
    public partial class ActiveUsersAggSettingsCommand : UsersAggSettingsSearchableCommand
    {
        public ActiveUsersAggSettingsCommand(ILogRequestContext ctx, UsersAggSettingsQueryModel query)
            : base(ctx, query) { }
    }
    public partial class DeactiveUsersAggSettingsCommand : UsersAggSettingsSearchableCommand
    {
        public DeactiveUsersAggSettingsCommand(ILogRequestContext ctx, UsersAggSettingsQueryModel query)
            : base(ctx, query) { }
    }
    public class UsersAggSettingsSearchableCommand : BaseSearchableCommand<UsersAggSettingsQueryModel> {
        public UsersAggSettingsSearchableCommand(ILogRequestContext ctx, UsersAggSettingsQueryModel query)
            : base(ctx, query) { }
    }

    public partial class CreateUserCommand : BaseRequestableCommand<UserQueryModel, UserDTO>
    {
        public bool UpdateIfExists { get; set; }
        public CreateUserCommand(ILogRequestContext ctx, UserDTO data, bool updateIfExists = true, UserQueryModel query = null) 
            : base(ctx, query, data) { this.UpdateIfExists = updateIfExists; }
    }
    public partial class DeleteUserCommand : BaseDeletionCommand<UserQueryModel>
    {
        public DeleteUserCommand(ILogRequestContext ctx, UserQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
      public partial class DeleteRangeUserCommand : BaseDeletionCommand<UserQueryModel>
    {
        public DeleteRangeUserCommand(ILogRequestContext ctx, UserQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
    public partial class UpdateRangeUserCommand : BaseRequestableRangeCommand<UserQueryModel, UserDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateRangeUserCommand(ILogRequestContext ctx, Dictionary<UserQueryModel, UserDTO> query)
            : base(ctx, query) { }
        public UpdateRangeUserCommand(ILogRequestContext ctx, UserQueryModel query, UserDTO data)
            : base(ctx, new Dictionary<UserQueryModel, UserDTO> { { query, data } }) { }
    }
    
    public partial class UpdateUserCommand : BaseRequestableCommand<UserQueryModel, UserDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateUserCommand(ILogRequestContext ctx, UserQueryModel query, UserDTO data, bool createIfNotExists = true)
            : base(ctx, query, data) { this.CreateIfNotExists = createIfNotExists; }
    }
    public partial class ActiveUserCommand : UserSearchableCommand
    {
        public ActiveUserCommand(ILogRequestContext ctx, UserQueryModel query)
            : base(ctx, query) { }
    }
    public partial class DeactiveUserCommand : UserSearchableCommand
    {
        public DeactiveUserCommand(ILogRequestContext ctx, UserQueryModel query)
            : base(ctx, query) { }
    }
    public class UserSearchableCommand : BaseSearchableCommand<UserQueryModel> {
        public UserSearchableCommand(ILogRequestContext ctx, UserQueryModel query)
            : base(ctx, query) { }
    }

}
