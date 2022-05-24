using UnityEngine;
using UnityEngine.Networking;

namespace PdUtils.MailService.Impl
{
    public class MailService : IMailService
    {
        public void Open(Mail mail)
        {
            var escapedMail = EscapeURL(mail.Email);
            var escapedSubject = EscapeURL(mail.Subject);
            var escapedSBody = EscapeURL(mail.Body);
            Application.OpenURL("mailto:" + escapedMail + "?subject=" + escapedSubject + "&body=" + escapedSBody);
        }
        
       private string EscapeURL (string text)
        {
            return UnityWebRequest.EscapeURL(text).Replace("+","%20");
        }
    }
}