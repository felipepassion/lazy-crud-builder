﻿using MediatR;
using System.Linq.Expressions;
using FluentValidation.Results;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Queries;
namespace LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Queries.Models
{
	using Filters;
    using Entities;
	public partial class MarketPlaceAggSettingsQueryModel : BaseQueryModel<MarketPlaceAggSettings> {
        public override Expression<Func<MarketPlaceAggSettings, bool>> GetFilter() => MarketPlaceAggSettingsFilters.GetFilters(this, IsOrSpecification==true);
		public int? UserIdEqual { get; set; }
		public int? UserIdNotEqual { get; set; }
		public int[]? UserIdContains { get; set; }
		public int[]? UserIdNotContains { get; set; }
		public int? UserIdSince { get; set; }
		public int? UserIdUntil { get; set; }
		public int? UserIdLessThan { get; set; }
		public int? UserIdGreaterThan { get; set; }
		public int? UserIdLessThanOrEqual { get; set; }
		public int? UserIdGreaterThanOrEqual { get; set; }
		public bool? AutoSaveSettingsEnabledEqual { get; set; }
		public System.DateTime? CreatedAtEqual { get; set; }
		public System.DateTime? CreatedAtNotEqual { get; set; }
		public System.DateTime[]? CreatedAtContains { get; set; }
		public System.DateTime[]? CreatedAtNotContains { get; set; }
		public System.DateTime? CreatedAtSince { get; set; }
		public System.DateTime? CreatedAtUntil { get; set; }
		public System.DateTime? CreatedAtLessThan { get; set; }
		public System.DateTime? CreatedAtGreaterThan { get; set; }
		public System.DateTime? CreatedAtLessThanOrEqual { get; set; }
		public System.DateTime? CreatedAtGreaterThanOrEqual { get; set; }
		public System.DateTime? UpdatedAtEqual { get; set; }
		public System.DateTime? UpdatedAtNotEqual { get; set; }
		public System.DateTime[]? UpdatedAtContains { get; set; }
		public System.DateTime[]? UpdatedAtNotContains { get; set; }
		public System.DateTime? UpdatedAtSince { get; set; }
		public System.DateTime? UpdatedAtUntil { get; set; }
		public System.DateTime? UpdatedAtLessThan { get; set; }
		public System.DateTime? UpdatedAtGreaterThan { get; set; }
		public System.DateTime? UpdatedAtLessThanOrEqual { get; set; }
		public System.DateTime? UpdatedAtGreaterThanOrEqual { get; set; }
		public System.DateTime? DeletedAtEqual { get; set; }
		public System.DateTime? DeletedAtNotEqual { get; set; }
		public System.DateTime[]? DeletedAtContains { get; set; }
		public System.DateTime[]? DeletedAtNotContains { get; set; }
		public System.DateTime? DeletedAtSince { get; set; }
		public System.DateTime? DeletedAtUntil { get; set; }
		public System.DateTime? DeletedAtLessThan { get; set; }
		public System.DateTime? DeletedAtGreaterThan { get; set; }
		public System.DateTime? DeletedAtLessThanOrEqual { get; set; }
		public System.DateTime? DeletedAtGreaterThanOrEqual { get; set; }
		public int? IdEqual { get; set; }
		public int? IdNotEqual { get; set; }
		public int[]? IdContains { get; set; }
		public int[]? IdNotContains { get; set; }
		public string? ExternalIdEqual { get; set; }
		public string? ExternalIdNotEqual { get; set; }
		public string? ExternalIdContains { get; set; }
		public string? ExternalIdStartsWith { get; set; }
		public bool? IsDeletedEqual { get; set; }
	}
}
