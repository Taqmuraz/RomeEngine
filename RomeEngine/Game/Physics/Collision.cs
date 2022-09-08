using System.Collections.Generic;

namespace RomeEngine
{
	public class Collision
	{
		List<ContactData> contacts;

		public ContactData this[int index]
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

		public void AddContact(ContactData contact)
		{
			if (contacts == null) contacts = new List<ContactData>();

			contacts.Add(contact);
		}
	}
}
