using System;

namespace UnityEngine.JWT
{
	public class SignatureVerificationException : Exception
	{
		public SignatureVerificationException(string message)
			: base(message)
		{
		}
	}
}