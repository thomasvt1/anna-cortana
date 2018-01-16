using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Luis;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace BotAnna.Dialogs
{

    [LuisModel("84df9d5d-46df-4c80-a7d7-8f1bd48def7b", "b6aaada782404ce0831ed7249766f528")]
    [Serializable]
    public class MeBotLuisDialog : LuisDialog<object>
    {
        protected string Note { get; set; }
        protected string Patient { get; set; }
        protected int PatientId { get; set; }



        [LuisIntent("Next Patient")]
        public async Task NextPatient(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Who are we helping next?");
            context.Wait(ChangePatient);
        }

        public async Task ChangePatient(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            this.Patient = (await result).Text;

            DatabaseConnector dbc = new DatabaseConnector();

            int tempPatientId = dbc.CheckPatient(Patient);

            if (tempPatientId > 0)
            {
                this.PatientId = tempPatientId;
                await context.PostAsync($"Sure we are helping {this.Patient} with patient id {this.PatientId} today");

                context.Wait(MessageReceived);
            }
            else
            {
                await context.PostAsync($"We don't know any patient with the name {this.Patient}");
                this.Patient = null;
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("None")]
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry. I didn't understand you.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("About Anna")]
        public async Task AboutMe(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hey, I'm Anna your personal assistant");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Make a Note")]
        public async Task MakeANote(IDialogContext context, LuisResult result)
        {
            if (this.Patient == null) { await context.PostAsync("No Patient found, who are we helping today?"); context.Wait(ChangePatient); return; }

            await context.PostAsync("What do you want to note?");

            context.Wait(MessageReceivedNote);  // ask what to note
        }

        public async Task MessageReceivedNote(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            this.Note = (await result).Text;

            DatabaseConnector dbc = new DatabaseConnector();

            dbc.AddNote(Note, PatientId);


            await context.PostAsync($"Your note: {this.Note} has been saved");

            context.Wait(MessageReceived);
        }

        // --- !!! make the what patient are we helping today, first !!! --- 
        [LuisIntent("Show Notes")]
        public async Task ShowNotes(IDialogContext context, LuisResult result)
        {
            if (this.Patient == null) { await context.PostAsync("No Patient found, who are we helping today?"); context.Wait(ChangePatient); return; }

            DatabaseConnector dbc = new DatabaseConnector();

            String noteTemp = dbc.GetNote(Patient, PatientId);

            await context.PostAsync("This is the last note: " + noteTemp);

            context.Wait(MessageReceived);
        }
    }

}