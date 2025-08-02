using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.ComponentModel;

namespace MoneyApp.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Stammdaten")]
    [XafDisplayName("Kategorie")]
    [XafDefaultProperty(nameof(Name))]
    [ImageName("BO_Category")]
    [CreatableItem(false)]
    public class Kategorie : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Kategorie(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        }
        public string Name
        {
            get => GetPropertyValue<string>(nameof(Name));
            set => SetPropertyValue(nameof(Name), value);
        }
        public KategorieTyp Typ
        {
            get => GetPropertyValue<KategorieTyp>(nameof(Typ));
            set => SetPropertyValue(nameof(Typ), value);
        }


    }
    public enum KategorieTyp
    {
        Konten,
        Buchungen

    }
}