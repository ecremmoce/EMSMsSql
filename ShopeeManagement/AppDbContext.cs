using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class AppDbContext : DbContext
    {
        public AppDbContext() : base()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDbContext, ShopeeManagement.Migrations.Configuration>());
        }
        public AppDbContext(string connectionStringOrName) : base(connectionStringOrName)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<DesignStyle> DesignStyles { get; set; }
        public DbSet<ShopeeAccount> ShopeeAccounts { get; set; }
        public DbSet<ShopeeCategory> ShopeeCategories { get; set; }
        public DbSet<ShopeeCategoryOnly> ShopeeCategorOnlys { get; set; }
        public DbSet<ProductLink> ProductLinks { get; set; }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }
        public DbSet<ShippingRateSlsSg> ShippingRateSlsSgs { get; set; }
        public DbSet<ShippingRateSlsId> ShippingRateSlsIds { get; set; }
        public DbSet<ShippingRateSlsPh> ShippingRateSlsPhs { get; set; }
        public DbSet<HFType> HFTypes { get; set; }
        public DbSet<HFList> HFLists { get; set; }
        public DbSet<TemplateHeader> TemplateHeaders { get; set; }
        public DbSet<TemplateFooter> TemplateFooters { get; set; }
        public DbSet<HeaderSeparator> HeaderSeparators { get; set; }
        public DbSet<FooterSeparator> FooterSeparators { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ShopeeVariationPrice> ShopeeVariationPrices { get; set; }
        public DbSet<CustomCategoryData> CustomCategoryDatas { get; set; }

        //원본 데이터용
        public DbSet<ItemInfo> ItemInfoes { get; set; }
        public DbSet<ItemVariation> ItemVariations { get; set; }
        public DbSet<ItemAttribute> ItemAttributes { get; set; }
        public DbSet<ItemLogistic> ItemLogistics { get; set; }
        public DbSet<ItemWholesale> ItemWholesales { get; set; }

        //업로드 데이터용
        public DbSet<ItemInfoDraft> ItemInfoDrafts { get; set; }

        public DbSet<Promotion> Promotions { get; set; }

        public DbSet<ConfigVar> ConfigVars { get; set; }

        public DbSet<SetHeader> SetHeaders { get; set; }
        public DbSet<SetFooter> SetFooters { get; set; }

        public DbSet<ItemVariationDraft> ItemVariationDrafts { get; set; }
        public DbSet<ItemAttributeDraft> ItemAttributeDrafts { get; set; }
        public DbSet<ItemAttributeDraftTar> ItemAttributeDraftTars { get; set; }
        public DbSet<ItemWholesaleDraft> ItemWholesaleDrafts { get; set; }

        public DbSet<CategoryVariationMandatoryCount> CategoryVariationMandatoryCounts { get; set; }

        public DbSet<Logistic> Logistics { get; set; }

        public DbSet<AllAttributeList> AllAttributeLists { get; set; }

        public DbSet<AttributeNameMap> AttributeNameMaps { get; set; }

        public DbSet<TitleType> TitleTypes { get; set; }
        public DbSet<TitleBrand> TitleBrands { get; set; }
        public DbSet<TitleModel> TitleModels { get; set; }
        public DbSet<TitleGroup> TitleGroups { get; set; }
        public DbSet<TitleFeature> TitleFeatures { get; set; }
        public DbSet<TitleOption> TitleOptions { get; set; }
        public DbSet<TitleRelat> TitleRelats { get; set; }

        public DbSet<ShippingRateYSMy> ShippingRateYSMys { get; set; }
        public DbSet<ShippingRateSlsMy> ShippingRateSlsMys { get; set; }
        public DbSet<ShippingRateDRTh> ShippingRateDRThs { get; set; }
        public DbSet<ShippingRateDRVn> ShippingRateDRVns { get; set; }
        public DbSet<ShippingRateYTOTw> ShippingRateYTOTws { get; set; }

        public DbSet<FavoriteCategoryData> FavoriteCategoryDatas { get; set; }
        public DbSet<FavKeyword> FavKeywords { get; set; }
        public DbSet<FavKeywordOther> FavKeywordOthers { get; set; }

        public DbSet<TemplateVersion> TemplateVersions { get; set; }
    }
}
