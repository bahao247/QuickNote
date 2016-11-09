using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace QuickNote1
{
    public partial class EditPage : ContentPage
    {
        private QuickNote quicknote;
        private int statusNoteSwitchPriority;
        private TimeZoneInfo localZone = TimeZoneInfo.Local;
        public EditPage(QuickNote quicknote)
        {
            InitializeComponent();
            this.quicknote = quicknote;

            saveButton.Clicked += SaveButton_Clicked;
            deleteButton.Clicked += DeleteButton_Clicked;
            statusNoteSwitchNone.Toggled += statusNoteSwitchNone_Toggled;
            statusNoteSwitchLow.Toggled += statusNoteSwitchLow_Toggled;
            statusNoteSwitchHigh.Toggled += statusNoteSwitchHigh_Toggled;
            statusNoteSwitchUrgen.Toggled += statusNoteSwitchUrgen_Toggled;

            nameNoteEntry.Text = quicknote.NameNote;
            statusNoteSwitch.IsToggled = quicknote.StatusNote;
            doneNoteSwitch.IsToggled = quicknote.DoneNote;
            typeNoteEntry.Text = quicknote.TypeNote;
            dueTimeNotePicker.Time = quicknote.DueTimeNote;

            dueDateNotePicker.Date = quicknote.DueDateNote.AddHours(-localZone.BaseUtcOffset.Hours);

            if ((statusNoteSwitch.IsToggled == true) && ((dueDateNotePicker.Date + dueTimeNotePicker.Time) <= DateTime.Now))
            {
                statusNoteSwitch.IsToggled = false;
            }
            else if ((statusNoteSwitch.IsToggled == false) && ((dueDateNotePicker.Date + dueTimeNotePicker.Time) > DateTime.Now))
            {
                statusNoteSwitch.IsToggled = true;
            }

            if (doneNoteSwitch.IsToggled)
            {
                remindingNoteEntry.Text = "Task done!!!";
            }
            else if (!statusNoteSwitch.IsToggled)
            {
                remindingNoteEntry.Text = "Task disable!!!";
            }
            else if ((dueDateNotePicker.Date + dueTimeNotePicker.Time) < DateTime.Now)
            {
                remindingNoteEntry.Text = "Expired task!!!";
            }
            else
            {
                remindingNoteEntry.Text = Math.Round(((dueDateNotePicker.Date + dueTimeNotePicker.Time - DateTime.Now).TotalMinutes), 0).ToString() + "min";
            }

            statusNoteSwitchPriority = quicknote.PriorityNote;
            if (statusNoteSwitchPriority == 3)
            {
                statusNoteSwitchUrgen.IsToggled = true;
            }
            else if (statusNoteSwitchPriority == 2)
            {
                statusNoteSwitchHigh.IsToggled = true;
            }
            else if (statusNoteSwitchPriority == 1)
            {
                statusNoteSwitchLow.IsToggled = true;
            }
            else if (statusNoteSwitchPriority == 0)
            {
                statusNoteSwitchNone.IsToggled = true;
            }

            textNoteEntry.Text = quicknote.TextNote;
        }

        private void statusNoteSwitchUrgen_Toggled(object sender, ToggledEventArgs e)
        {
            if (e.Value == true)
            {
                statusNoteSwitchPriority = 3;
                statusNoteSwitchNone.IsToggled = false;
                statusNoteSwitchLow.IsToggled = false;
                statusNoteSwitchHigh.IsToggled = false;
            }
        }

        private void statusNoteSwitchHigh_Toggled(object sender, ToggledEventArgs e)
        {
            if (e.Value == true)
            {
                statusNoteSwitchPriority = 2;
                statusNoteSwitchNone.IsToggled = false;
                statusNoteSwitchLow.IsToggled = false;
                statusNoteSwitchUrgen.IsToggled = false;
            }
        }

        private void statusNoteSwitchLow_Toggled(object sender, ToggledEventArgs e)
        {
            if (e.Value == true)
            {
                statusNoteSwitchPriority = 1;
                statusNoteSwitchNone.IsToggled = false;
                statusNoteSwitchHigh.IsToggled = false;
                statusNoteSwitchUrgen.IsToggled = false;
            }
        }

        private void statusNoteSwitchNone_Toggled(object sender, ToggledEventArgs e)
        {
            if (e.Value == true)
            {
                statusNoteSwitchPriority = 0;
                statusNoteSwitchLow.IsToggled = false;
                statusNoteSwitchHigh.IsToggled = false;
                statusNoteSwitchUrgen.IsToggled = false;
            }
        }

        public async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var rta = await DisplayAlert("Confirm", "Do you want to delete this note?", "Yes", "No");
            if (!rta) return;

            using (var data = new DataAccess())
            {
                data.DeleteNote(quicknote);
            }

            await DisplayAlert("Confirm", "Note was deleted successfully", "OK");
            await Navigation.PushAsync(new HomePage());
        }

        public async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameNoteEntry.Text))
            {
                await DisplayAlert("Error", "You must enter names", "Accept");
                nameNoteEntry.Focus();
                return;
            }

            if (string.IsNullOrEmpty(typeNoteEntry.Text))
            {
                await DisplayAlert("Error", "You must enter type", "Accept");
                typeNoteEntry.Focus();
                return;
            }

            if (statusNoteSwitch.IsToggled == true && ((dueDateNotePicker.Date + dueTimeNotePicker.Time) <= DateTime.Now))
            {
                statusNoteSwitch.IsToggled = false;
            }
            else if (statusNoteSwitch.IsToggled == false && ((dueDateNotePicker.Date + dueTimeNotePicker.Time) > DateTime.Now))
            {
                statusNoteSwitch.IsToggled = true;
            }

            QuickNote quicknote = new QuickNote
            {
                IDNote = this.quicknote.IDNote,
                NameNote = nameNoteEntry.Text,
                StatusNote = statusNoteSwitch.IsToggled,
                DoneNote = doneNoteSwitch.IsToggled,
                TypeNote = typeNoteEntry.Text,
                DueTimeNote = dueTimeNotePicker.Time,
                DueDateNote = (dueDateNotePicker.Date + dueTimeNotePicker.Time).AddHours(localZone.BaseUtcOffset.Hours),
                PriorityNote = statusNoteSwitchPriority,
                TextNote = textNoteEntry.Text
            };

            using (var data = new DataAccess())
            {
                data.UpdateNote(quicknote);
            }

            if (!doneNoteSwitch.IsToggled && ((dueDateNotePicker.Date + dueTimeNotePicker.Time) <= DateTime.Now))
            {

                var rta = await DisplayAlert("Confirm", "Expired task. Do you want to delete this note?", "Yes", "No");
                if (rta)
                    using (var data = new DataAccess())
                    {
                        data.DeleteNote(quicknote);
                    }
            }

            await DisplayAlert("Confirm", "Content of note updated correctly", "Accept");
            await Navigation.PushAsync(new HomePage());

        }
    }
}
