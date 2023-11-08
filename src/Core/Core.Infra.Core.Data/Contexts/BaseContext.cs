using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using LazyCrud.Core.Application.DTO.Attributes;
using LazyCrud.Core.Application.DTO.Extensions;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Events;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.ValueObjects;
using LazyCrud.Core.Domain.Attributes.T4;
using LazyCrud.Core.Domain.CrossCutting;
using LazyCrud.Core.Domain.Seedwork;
using LazyCrud.Core.Infra.Data.Extensions;
using LazyCrud.CrossCuting.Infra.Utils.Extensions;
using LazyCrud.CrossCutting.Infra.Log.Contexts;

namespace LazyCrud.Core.Infra.Data.Contexts
{
    public class BaseContext : DbContext, IUnitOfWork
    {
        public bool IsEmpty<T>(T entity) where T : class => this.Find<T>() == null;

        IMediator _mediator;
        private readonly IServiceProvider _scope;

        public BaseContext(IMediator mediator, DbContextOptions options, IServiceProvider scope)
            : base(options)
        {
            _mediator = mediator;
            _scope = scope;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public bool Commit(object data = null)
        {
            return CommitAsync(data).Result?.Success == true;
        }

        public async Task<DomainResponse> CommitAsync(object data)
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            try
            {
                await SaveChangesAsync();

                await _mediator.PublishDomainEvents(this).ConfigureAwait(false);

                return new DomainResponse(data);
            }
            catch (Exception ex)
            {
                var context = _scope.GetRequiredService<ILogRequestContext>();
                await _mediator.Publish(new ErrorEvent(context, ex, "Erro de banco de dados", data));
                return new DomainResponse(ex);
            }
        }

        public async Task<bool> ExecuteNpCommand(string rawText)
        {
            try
            {
                await this.Database.ExecuteSqlRawAsync(new SqlCommand(rawText).CommandText);
                return true;
            }
            catch (Exception ex)
            {
                // log error
                return false;
            }
        }

        public void ResolveAttaches<T>(T entity)
            where T : Entity
        {
            //this.ChangeTracker.Clear();

            foreach (var ent in from item in entity.ExtractPropertyInfos("ListingPicker")
                                from ent in item.Value as IEnumerable<IEntity>
                                where item?.Value is not null
                                select (ent))
            {
                //Entry(User).State = EntityState.Detached;
                //var local = this.ChangeTracker.Entries().FirstOrDefault(x => x.Entity.GetType() == ent.GetType());
                // if (local == null)
                try
                {
                    this.Attach(ent);
                }
                catch (Exception)
                {
                }
            }

            var test1 = entity.GetType().FillFields(true);
            var test2 = entity.GetPropertiesByWithAttribute<IgnorePropertyT4>();
            foreach (var ent in test2)
            {
                ent.SetValue(entity, null);
            }
            foreach (var ent in entity.GetPropertiesByWithAttribute<DettachNavigationProperty>())
            {
                ent.SetValue(entity, null);
            }
        }

        public void ResolveAttachesOnUpdate<T, K>(T entity, K entityDTO)
            where T : Entity, new()
            where K : Entity, new()
        {
            var properties = entity.ExtractPropertyInfos("ListingPicker").GroupBy(x => x.info.Name)
                .Select(x => x.FirstOrDefault());
            foreach (var item in properties)
            {
                if (item!.info.CustomAttributes.Any(x => x.AttributeType.Name == "IgnorePropertyT4"))
                    continue;
                var itemVal = item.info?.GetValue(entity);
                if (itemVal == null)
                    itemVal = new List<IEntity>();
                var propOnOther = entityDTO.GetType().GetProperty(item.info!.Name)?.GetValue(entityDTO) as IEnumerable<IEntity> ?? new List<IEntity>();

                var existingProps = itemVal as IEnumerable<IEntity> ?? new List<IEntity>();
                var propOnOtherEmpty = !propOnOther.Any();

                var deletedProps = existingProps.Where(x => propOnOtherEmpty || !propOnOther.Any(p => p.Id == x.Id));
                var remainingProps = existingProps.Where(x => !propOnOtherEmpty && propOnOther.Any(p => p.Id == x.Id));

                System.Collections.IList list = (System.Collections.IList)item.info.GetValue(entity);

                if (deletedProps.Any())
                {
                    object instance = Activator.CreateInstance(item.info.PropertyType);
                    foreach (var deleted in deletedProps)
                    {
                        try
                        {
                            var test1 = deleted.ExtractProperties().ToArray();
                            var myField = deleted.ExtractProperties().FirstOrDefault(x => x.PropertyType.GetGenericArguments()?.FirstOrDefault() == entity.GetType());
                            //var str = @$"Delete from {'"'}{entity.GetType().Name + item.info.PropertyType.GetGenericArguments()[0].Name}{'"'} where {'"'}{item.info.Name + "Id"}{'"'}={deleted.Id} AND {'"'}{myField.Name + "Id"}{'"'}={entity.Id}";
                            //var cmd = $"SELECT ""CustomerNumber"" FROM dbo.""Customer"" WHERE ""Id"" = { id}AND ""CustomerNumber"" = ANY({ listOfParams})";
                            var test = new SqlCommand($"delete from \"{entity.GetType().Name.ToLower()}{item?.info?.PropertyType?.GetGenericArguments()[0].Name.ToLower()}\" where \"{item.info.Name.ToLower() + "id"}\"={deleted.Id} and \"{(myField?.Name.ToLower() ?? entity.GetType().Name.ToLower()) + "id"}\"={entity.Id}");
                            this.Database.ExecuteSqlRaw($"{test.CommandText}");
                        }
                        catch (Exception)
                        {
                            //var test1 = deleted.ExtractProperties().ToArray();
                            //var myField = deleted.ExtractProperties().FirstOrDefault(x => x.PropertyType.GetGenericArguments()?.FirstOrDefault() == entity.GetType());
                            //var str = @$"Delete from {'"'}{entity.GetType().Name + item.info.PropertyType.GetGenericArguments()[0].Name}{'"'} where {'"'}{item.info.Name + "Id"}{'"'}={deleted.Id} AND {'"'}{myField.Name + "Id"}{'"'}={entity.Id}";
                            //var cmd = $"SELECT ""CustomerNumber"" FROM dbo.""Customer"" WHERE ""Id"" = { id}AND ""CustomerNumber"" = ANY({ listOfParams})";
                            var test = new SqlCommand($"delete from \"{item?.info?.PropertyType?.GetGenericArguments()[0].Name.ToLower()}\" where \"{"id"}\"={deleted.Id}");
                            this.Database.ExecuteSqlRaw($"{test.CommandText}");
                        }
                        Entry(deleted).State = EntityState.Detached;
                        //list[list.IndexOf(deleted)] = null;
                        //list.Remove(deleted);
                    }
                    // List<T> implements the non-generic IList interface
                    list.Clear();
                    //item.info.SetValue(entity, list, );

                    //item.info.SetValue(remainingProps, entity);
                }

                foreach (var notTouchProp in remainingProps)
                {
                    Entry(notTouchProp).State = EntityState.Detached;
                    //var properties2 = notTouchProp.ExtractPropertyInfos("ListingPicker")
                    //   .GroupBy(x => x.info.Name).Select(x => x.FirstOrDefault())
                    //   .Where(x => x.info.Name != item.info.Name);

                    //var meOnOther = propOnOther.FirstOrDefault(x => x.Id == notTouchProp.Id);

                    //notTouchProp.Update(meOnOther, properties2.Select(x => x.info.Name).ToArray());

                    //foreach (var ent in notTouchProp.GetPropertiesByWithAttribute<DettachNavigationProperty>())
                    //{
                    //    ent.SetValue(notTouchProp, null);
                    //}

                    //Entry(notTouchProp).State = EntityState.Modified;

                    //this.Attach(notTouchProp);
                    //ResolveAttachesOnUpdate(notTouchProp as Entity, meOnOther as Entity);

                    //foreach (var prop in properties2)
                    //{
                    //    //var other = (propOnOther.FirstOrDefault(x => x.Id == notTouchProp.Id).ExtractPropertyInfos().FirstOrDefault(x => x.info.Name == prop.Key).Value as IEnumerable<IEntity>).First();
                    //    //prop.FirstOrDefault().info.SetValue(prop.FirstOrDefault().Value as IEntity, other);
                    //}

                    //foreach (var item2 in propOnOther)
                    //{
                    //    //Entry(item2).State = EntityState.Added;
                    //
                    //    //var propOnOther2 = entityDTO.FindPropertyInfo(item2.info.Name).Value as IEnumerable<IEntity> ?? new List<IEntity>();
                    //    //item2.info.SetValue(notTouchProp, propOnOther2);
                    //}
                    ////list.Remove(notTouchProp);
                }

                //list = new List<IEntity>();

                foreach (var entryToAdd in propOnOther.Where(x => !deletedProps.Any(p => p.Id == x.Id) && !remainingProps.Any(p => p.Id == x.Id)))
                {
                    if (entryToAdd.Id == 0)
                    {
                        foreach (var ent in entryToAdd.GetPropertiesByWithAttribute<DettachNavigationProperty>())
                        {
                            ent.SetValue(entryToAdd, null);
                        }
                    }
                    else
                    {
                        try
                        {
                            this.Attach(entryToAdd);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    foreach (var ent in entryToAdd.GetPropertiesByWithAttribute<ListingPicker>())
                    {
                        var val = ent.GetValue(entryToAdd) as IEnumerable<IEntity>;
                        //foreach (var sub in val)
                        //{
                        //    this.Attach(sub);
                        //}
                    }
                    list!.Add(entryToAdd);
                }

                item.info.SetValue(entityDTO, list);
            }

            foreach (var ent in entity.GetPropertiesByWithAttribute<ListingPicker>())
            {
                if (!ent.CustomAttributes.Any(x => x.AttributeType.Name == "AttatchOnUpdate"))
                    ent.SetValue(entity, null);
                else
                {
                    if(ent is IEnumerable<Entity> list)
                    {
                        
                    }
                    foreach (var subitem in ent.GetPropertiesByWithAttribute<AttatchOnUpdate>())
                    {
                        this.Attach(subitem);
                    }
                }
            }

            foreach (var ent in entity.GetPropertiesByWithAttribute<ForeignDataSelectorDropDownWithAutoComplete>())
            {
                ent.SetValue(entity, null);
            }

            foreach (var ent in entity.GetPropertiesByWithAttribute<IgnorePropertyT4>())
            {
                ent.SetValue(entity, null);
            }

            

            foreach (var ent in entity.GetPropertiesByWithAttribute<DettachNavigationProperty>())
            {
                ent.SetValue(entity, null);
            }

            Entry(entity).State = EntityState.Modified;
        }
    }
}



