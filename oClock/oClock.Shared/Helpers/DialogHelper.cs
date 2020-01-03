using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace oClock.Shared.Helpers
{
    public static class DialogHelper
    {
        /// <summary>
        /// To Display MessageBox
        /// </summary>
        /// <param name="content">Message to Display</param>
        /// <param name="title">Title Of MessageBox</param>
        /// <returns></returns>
        public static async Task AlertAsync(string content, string title)
        {
            await new MessageDialog(content, title).ShowAsync();
        }
        /// <summary>
        /// To Display MessageBox
        /// </summary>
        /// <param name="content">Message to Display</param>
        /// <returns></returns>
        public static async Task AlertAsync(string content)
        {
            await new MessageDialog(content).ShowAsync();
        }
        public static async Task<DialogResults> ConfirmAsync(string content, string title, DialogButtons dialogButtons)
        {
            var messageDialog = new MessageDialog(content, title);

            switch (dialogButtons)
            {
                case DialogButtons.YesNo:
                    messageDialog.Commands.Add(new UICommand(DialogResults.Yes.ToString(), null, DialogResults.Yes));
                    messageDialog.Commands.Add(new UICommand(DialogResults.No.ToString(), null, DialogResults.No));
                    messageDialog.DefaultCommandIndex = 1;

                    break;
                case DialogButtons.YesNoCancel:
                    messageDialog.Commands.Add(new UICommand(DialogResults.Yes.ToString(), null, DialogResults.Yes));
                    messageDialog.Commands.Add(new UICommand(DialogResults.No.ToString(), null, DialogResults.No));
                    messageDialog.Commands.Add(new UICommand(DialogResults.Cancel.ToString(), null, DialogResults.Cancel));
                    messageDialog.DefaultCommandIndex = 2;
                    break;
                case DialogButtons.OkCancel:
                    messageDialog.Commands.Add(new UICommand(DialogResults.Ok.ToString(), null, DialogResults.Ok));
                    messageDialog.Commands.Add(new UICommand(DialogResults.Cancel.ToString(), null, DialogResults.Cancel));
                    messageDialog.DefaultCommandIndex = 1;
                    break;
                default:
                    messageDialog.Commands.Add(new UICommand(DialogResults.Ok.ToString(), null, DialogResults.Ok));
                    messageDialog.DefaultCommandIndex = 0;
                    break;
            }
            var commandChosen = await messageDialog.ShowAsync();
            return (DialogResults)commandChosen.Id;
        }
    }
}
