using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QuickNote1
{
    class QuickNoteCellFilterDay: ViewCell
    {
        public QuickNoteCellFilterDay()
        {

            var IDQuickNoteLabel = new Label
            {
                TextColor = Color.Blue,
                Font = Font.BoldSystemFontOfSize(NamedSize.Large),
                HorizontalOptions = LayoutOptions.Start
            };
            IDQuickNoteLabel.SetBinding(Label.TextProperty, new Binding("IDNote"));

            var NameQuickNoteLabel = new Label
            {
                TextColor = Color.Blue,
                Font = Font.BoldSystemFontOfSize(NamedSize.Medium),
                HorizontalOptions = LayoutOptions.Start
            };
            NameQuickNoteLabel.SetBinding(Label.TextProperty, new Binding("NameNote"));

            var TypeQuickNoteLabel = new Label
            {
                TextColor = Color.Blue,
                Font = Font.BoldSystemFontOfSize(NamedSize.Medium),
                HorizontalOptions = LayoutOptions.Start
            };
            TypeQuickNoteLabel.SetBinding(Label.TextProperty, new Binding("TypeNote"));

            var DueDateNoteLabel = new Label
            {
                TextColor = Color.Blue,
                Font = Font.BoldSystemFontOfSize(NamedSize.Large),
                HorizontalOptions = LayoutOptions.Start
            };
            DueDateNoteLabel.SetBinding(Label.TextProperty, new Binding("DueDateNote"));

            var DoneNoteSwitch = new Switch
            {
                HorizontalOptions = LayoutOptions.End
            };
            DoneNoteSwitch.SetBinding(Switch.IsToggledProperty, new Binding("DoneNote"));

            var panel1 = new StackLayout
            {
                Children = { IDQuickNoteLabel, DueDateNoteLabel, DoneNoteSwitch },
                Orientation = StackOrientation.Horizontal

            };

            var panel2 = new StackLayout
            {
                Children = { NameQuickNoteLabel, TypeQuickNoteLabel },
                Orientation = StackOrientation.Horizontal
            };

            View = new StackLayout
            {
                Children = { panel1, panel2 },
                Orientation = StackOrientation.Vertical
            };
        }
    }
}
