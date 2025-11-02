
using CsvHelper.Configuration;
namespace Niu.Nutri.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Profiles
{
	using Application.DTO.Aggregates.DefaultTemplateAgg.Requests;
	using Entities;
	public partial class DefaultEntityCsvMap : ClassMap<DefaultEntity>
	{
		public DefaultEntityCsvMap()
		{
			
		}
	}
	public partial class DefaultTemplateAggSettingsCsvMap : ClassMap<DefaultTemplateAggSettings>
	{
		public DefaultTemplateAggSettingsCsvMap()
		{
			Map(x=>x.UserId).Name("UserId");
		}
	}
}
