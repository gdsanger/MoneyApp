using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;

namespace MoneyApp.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("MoneyApp")]
    [XafDisplayName("Buchung")]
    [ImageName("BO_Note")]
    public class Buchung : BaseObject, ISupportNotifications
    {
        public Buchung(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private Konto konto;
        [RuleRequiredField]
        [Association("Konto-Buchungen")]
        public Konto Konto
        {
            get => konto;
            set => SetPropertyValue(nameof(Konto), ref konto, value);
        }

        private int position;
        public int Position
        {
            get => position;
            protected set => SetPropertyValue(nameof(Position), ref position, value);
        }

        private DateTime datum;
        [RuleRequiredField]
        public DateTime Datum
        {
            get => datum;
            set => SetPropertyValue(nameof(Datum), ref datum, value);
        }

        private Kategorie kategorie;
        [DataSourceCriteria("Typ = ##Enum#MoneyApp.Module.BusinessObjects.KategorieTyp,Buchungen#")]
        public Kategorie Kategorie
        {
            get => kategorie;
            set => SetPropertyValue(nameof(Kategorie), ref kategorie, value);
        }

        private Buchungstyp typ;
        public Buchungstyp Typ
        {
            get => typ;
            set => SetPropertyValue(nameof(Typ), ref typ, value);
        }

        private DateTime faelligkeit;
        public DateTime Faelligkeit
        {
            get => faelligkeit;
            set => SetPropertyValue(nameof(Faelligkeit), ref faelligkeit, value);
        }

        private string zweck;
        [RuleRequiredField]
        public string Zweck
        {
            get => zweck;
            set => SetPropertyValue(nameof(Zweck), ref zweck, value);
        }

        private FileData datei;
        public FileData Datei
        {
            get => datei;
            set => SetPropertyValue(nameof(Datei), ref datei, value);
        }

        private decimal betrag;
        [RuleRequiredField]
        public decimal Betrag
        {
            get => betrag;
            set => SetPropertyValue(nameof(Betrag), ref betrag, value);
        }

        private decimal saldo;
        public decimal Saldo
        {
            get => saldo;
            protected set => SetPropertyValue(nameof(Saldo), ref saldo, value);
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (Konto != null)
            {
                if (Session.IsNewObject(this))
                {
                    var maxPos = Session.Evaluate<Buchung>(CriteriaOperator.Parse("Max(Position)"), CriteriaOperator.Parse("Konto = ?", Konto));
                    Position = (maxPos is int ? (int)maxPos : 0) + 1;
                }

                decimal sum = 0m;
                foreach (Buchung buchung in Konto.Buchungen)
                {
                    sum += buchung == this ? Betrag : buchung.Betrag;
                }
                Saldo = sum;
            }
        }

        DateTime ISupportNotifications.AlarmTime => Faelligkeit;

        string ISupportNotifications.Caption => $"{Zweck} {Faelligkeit:yyyy-MM-dd}";

        Guid ISupportNotifications.UniqueId => Oid;
    }

    public enum Buchungstyp
    {
        Geplant,
        Erledigt,
        Zurueckgestellt
    }
}

