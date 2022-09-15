using System;

namespace KittyHelper
{
    public static partial class KittyHelper
    {
        public static partial class KittyViewHelper
        {
            public class VueTable : VueElement
            {
                public VueTable(string textContent = "", params VueAttribute[] attributes) : base(
                    new VueTag("b-table", attributes), textContent)
                {
                }
            }
        }

        public static partial class KittyViewHelper
        {
            public class VueTd : VueElement
            {
                public static VueTd Default => new VueTd();

                public VueTd( params VueAttribute[] attributes) : base(
                    new VueTag("b-td", attributes))
                {
                }
                public VueTd(string textContent , params VueAttribute[] attributes) : base(
                    new VueTag("b-td",  attributes),textContent)
                {
                }
            }
        }

    

        public static partial class KittyViewHelper
        {
            public class VueBTr : VueElement
            {
      

                public VueBTr(params VueAttribute[] attributes) : base(
                    new VueTag("b-tr", attributes))
                {
                }
            }
        }

        public static partial class KittyViewHelper
        {
            public class VueBHead : VueElement
            {
                public static VueBHead Default => new VueBHead(new VueAttribute(":head-variant", "dark"));

                public VueBHead(params VueAttribute[] attributes) : base(
                    new VueTag("b-thead", attributes))
                {
                }
            }
        }
        public static partial class KittyViewHelper
        {
            public class VueBTh : VueElement
            {
        

                public VueBTh(string _stringContent, params VueAttribute[] attributes) : base(
                    new VueTag("b-th", attributes),_stringContent)
                {
                }
            }
        }
        public static partial class KittyViewHelper
        {
            public class VueBTBody : VueElement
            {
 

                public VueBTBody(params VueAttribute[] attributes) : base(
                    new VueTag("b-tbody", attributes))
                {
                }
            }
        }

        public static partial class KittyViewHelper
        {
            public class VueTableSimple : VueElement
            {
                public static VueTableSimple Default =>
                    new VueTableSimple(new VueAttribute(":hover", "true"));

                public VueTableSimple(params VueAttribute[] attributes) : base(
                    new VueTag("b-table-simple", attributes))
                {
                }
            }
        }

        public static partial class KittyViewHelper
        {
            public class VueBAlert : VueElement
            {
                public VueBAlert(string textContent = "", params VueAttribute[] attributes) : base(
                    new VueTag("b-alert", attributes), textContent)
                {
                }
            }
        }
    }
}