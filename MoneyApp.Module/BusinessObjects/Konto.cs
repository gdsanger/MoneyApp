using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MoneyApp.Module.BusinessObjects
{
    [NavigationItem("MoneyApp")]
    [XafDisplayName("Konto")]
    [XafDefaultProperty(nameof(Name))]
    [ImageName("BO_Account")]
    [DefaultClassOptions]
    [CreatableItem(false)]
    [Appearance("KontoSaldoNegativ", Criteria = "Saldo < 0", FontColor = "Red", TargetItems = "Saldo")]
    [Appearance("KontoSaldoErwartetNegativ", Criteria = "SaldoErwartet < 0", FontColor = "Red", TargetItems = "SaldoErwartet")]
    public class Konto : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Konto(Session session)
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
        public Kategorie Kategorie
        {
            get => GetPropertyValue<Kategorie>(nameof(Kategorie));
            set => SetPropertyValue(nameof(Kategorie), value);
        }

        [Association("Konto-Buchungen")]
        public XPCollection<Buchung> Buchungen
        {
            get => GetCollection<Buchung>(nameof(Buchungen));
        }

        [NonPersistent]
        public decimal Saldo => Buchungen.Where(b => b.Typ == Buchungstyp.Erledigt).Sum(b => b.Betrag);

        [NonPersistent]
        public decimal SaldoErwartet => Buchungen.Where(b => b.Typ == Buchungstyp.Erledigt || b.Typ == Buchungstyp.Geplant).Sum(b => b.Betrag);

        [Action(Caption = "Buchungen erzeugen", ImageName = "BO_Account")]
        public void ErzeugeBuchungen(ErzeugeBuchungenArgs args)
        {
            for (int i = 0; i < args.Anzahl; i++)
            {
                var buchung = new Buchung(Session)
                {
                    Konto = this,
                    Datum = args.Start.AddMonths(i * args.IntervallMonate),
                    AlarmTime = args.Start.AddMonths(i * args.IntervallMonate),
                    Typ = args.Status,
                    Kategorie = Session.GetObjectByKey<Kategorie>(args.Kategorie.Oid),
                    Zweck = $"{args.Zweck} ({i})",
                    Betrag = args.Betrag,
                };
                Session.Save(buchung);
            }
        }
    }

    [NonPersistent]
    public class ErzeugeBuchungenArgs
    {
        public decimal Betrag { get; set; }
        public DateTime Start { get; set; } = DateTime.Today;
        [DataSourceCriteria("Typ = ##Enum#MoneyApp.Module.BusinessObjects.KategorieTyp,Buchungen#")]
        public Kategorie Kategorie { get; set; }
        public int Anzahl { get; set; }
        public int IntervallMonate { get; set; }
        public Buchungstyp Status { get; set; } = Buchungstyp.Geplant; // Standardwert auf Geplant setzen
        public string Zweck { get; set; } = "Automatische Buchung";
    }
}