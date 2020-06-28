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
            /*
            modelBuilder.Entity<HFType>().HasKey(o => new { o.HFTypeID, o.UserId });
            modelBuilder.Entity<HFType>().Property(o => o.HFTypeID)
                .HasColumnOrder(0)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<HFType>().Property(o => o.UserId)
                .HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            
            
            modelBuilder.Entity<HFList>().HasKey(o => new { o.HFListID, o.UserId });
            modelBuilder.Entity<HFList>().Property(o => o.HFTypeID)
                .HasColumnOrder(0)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<HFList>().Property(o => o.UserId)
                .HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<HFList>().Property(o => o.HFListID)
                .HasColumnOrder(2)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<HFList>()
                .HasRequired<HFType>(o => o.HFType)
                .WithMany(o => o.HFLists)
                .HasForeignKey(o => new { o.HFTypeID, o.UserId });
            

            
            modelBuilder.Entity<ItemInfo>().HasKey(o => new { o.item_id, o.UserId });
            modelBuilder.Entity<ItemInfo>().Property(o => o.item_id)
                .HasColumnOrder(0)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemInfo>().Property(o => o.UserId)
                .HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            
            
            modelBuilder.Entity<ItemAttribute>().HasKey(o => new { o.item_id, o.UserId, o.attribute_id });
            modelBuilder.Entity<ItemAttribute>().Property(o => o.item_id)
                .HasColumnOrder(0)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemAttribute>().Property(o => o.UserId)
                .HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemAttribute>().Property(o => o.attribute_id)
                .HasColumnOrder(2)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemAttribute>()
                .HasRequired<ItemInfo>(o => o.ItemInfo)
                .WithMany(o => o.ItemAttributes)
                .HasForeignKey(o => new { o.item_id, o.UserId });
            
            
            modelBuilder.Entity<ItemLogistic>().HasKey(o => new { o.item_id, o.UserId, o.logistic_id });
            modelBuilder.Entity<ItemLogistic>().Property(o => o.item_id)
                .HasColumnOrder(0)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemLogistic>().Property(o => o.UserId)
                .HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemLogistic>().Property(o => o.logistic_id)
                .HasColumnOrder(2)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemLogistic>()
                .HasRequired<ItemInfo>(o => o.ItemInfo)
                .WithMany(o => o.ItemLogistics)
                .HasForeignKey(o => new { o.item_id, o.UserId });
            
            
            modelBuilder.Entity<ItemVariation>().HasKey(o => new { o.item_id, o.UserId, o.variation_id });
            modelBuilder.Entity<ItemVariation>().Property(o => o.item_id)
                .HasColumnOrder(0)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemVariation>().Property(o => o.UserId)
                .HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemVariation>().Property(o => o.variation_id)
                .HasColumnOrder(2)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemVariation>()
                .HasRequired<ItemInfo>(o => o.ItemInfo)
                .WithMany(o => o.ItemVariations)
                .HasForeignKey(o => new { o.item_id, o.UserId });
            
            
            modelBuilder.Entity<ItemWholesale>().HasKey(o => new { o.Idx, o.UserId });
            modelBuilder.Entity<ItemWholesale>().Property(o => o.item_id)
                .HasColumnOrder(0)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemWholesale>().Property(o => o.UserId)
                .HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemWholesale>().Property(o => o.Idx)
                .HasColumnOrder(2)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ItemWholesale>()
                .HasRequired<ItemInfo>(o => o.ItemInfo)
                .WithMany(o => o.ItemWholesales)
                .HasForeignKey(o => new { o.item_id, o.UserId });
            
            
            modelBuilder.Entity<ItemInfoDraft>().HasKey(o => new { o.Id, o.UserId });
            modelBuilder.Entity<ItemInfoDraft>().Property(o => o.Id)
                .HasColumnOrder(0)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ItemInfoDraft>().Property(o => o.UserId)
                .HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            
            
            modelBuilder.Entity<ItemAttributeDraft>().HasKey(o => new { o.ItemInfoDraftId, o.UserId, o.attribute_id });
            modelBuilder.Entity<ItemAttributeDraft>().Property(o => o.ItemInfoDraftId)
                .HasColumnOrder(0)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemAttributeDraft>().Property(o => o.UserId)
                .HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemAttributeDraft>().Property(o => o.attribute_id)
                .HasColumnOrder(2)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemAttributeDraft>()
                .HasRequired<ItemInfoDraft>(o => o.ItemInfoDraft)
                .WithMany(o => o.ItemAttributeDrafts)
                .HasForeignKey(o => new { o.ItemInfoDraftId, o.UserId });
            
            
            modelBuilder.Entity<ItemAttributeDraftTar>().HasKey(o => new { o.ItemInfoDraftId, o.UserId, o.attribute_id });
            modelBuilder.Entity<ItemAttributeDraftTar>().Property(o => o.ItemInfoDraftId)
                .HasColumnOrder(0)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemAttributeDraftTar>().Property(o => o.UserId)
                .HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemAttributeDraftTar>().Property(o => o.attribute_id)
                .HasColumnOrder(2)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemAttributeDraftTar>()
                .HasRequired<ItemInfoDraft>(o => o.ItemInfoDraft)
                .WithMany(o => o.ItemAttributeDraftTars)
                .HasForeignKey(o => new { o.ItemInfoDraftId, o.UserId });
            
            
            modelBuilder.Entity<ItemVariationDraft>().HasKey(o => new { o.ItemInfoDraftId, o.UserId, o.variation_id });
            modelBuilder.Entity<ItemVariationDraft>().Property(o => o.ItemInfoDraftId)
                .HasColumnOrder(0)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemVariationDraft>().Property(o => o.UserId)
                .HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemVariationDraft>().Property(o => o.variation_id)
                .HasColumnOrder(2)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemVariationDraft>()
                .HasRequired<ItemInfoDraft>(o => o.ItemInfoDraft)
                .WithMany(o => o.ItemVariationDrafts)
                .HasForeignKey(o => new { o.ItemInfoDraftId, o.UserId });
            
            
            modelBuilder.Entity<ItemWholesaleDraft>().HasKey(o => new { o.Idx, o.UserId });
            modelBuilder.Entity<ItemWholesaleDraft>().Property(o => o.ItemInfoDraftId)
                .HasColumnOrder(0)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemWholesaleDraft>().Property(o => o.UserId)
                .HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ItemWholesaleDraft>().Property(o => o.Idx)
                .HasColumnOrder(2)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ItemWholesaleDraft>()
                .HasRequired<ItemInfoDraft>(o => o.ItemInfoDraft)
                .WithMany(o => o.ItemWholesaleDrafts)
                .HasForeignKey(o => new { o.ItemInfoDraftId, o.UserId });
            */
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
