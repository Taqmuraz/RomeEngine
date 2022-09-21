using System.Collections.Generic;

namespace RomeEngine
{
	public class Collision2D
	{
		List<ContactData2D> contacts;

		public ContactData2D this[int index]
		{
			get
			{
				if (contacts == null) throw new System.IndexOutOfRangeException();
				else return contacts[index];
			}
			set
			{
				contacts[index] = value;
			}
		}
		public int contancsCount => contacts == null ? 0 : contacts.Count;

		public void AddContact(ContactData2D contact)
		{
			if (contacts == null) contacts = new List<ContactData2D>();

			contacts.Add(contact);
		}
	}
}
