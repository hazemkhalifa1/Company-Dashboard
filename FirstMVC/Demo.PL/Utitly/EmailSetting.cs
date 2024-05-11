using Demo.DAL.Entities;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Utitly
{
	public static class EmailSetting
	{
		public static void SendEmail(Email email)
		{
			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("hazemkhalifa1211@gmail.com", "szugfxbasbcqhcyz");
			client.Send("hazemkhalifa1211@gmail.com", email.Recipient, email.Subject, email.Body);

		}
	}
}
