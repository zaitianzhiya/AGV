using System;
using System.Xml;

namespace Canvas
{
	internal interface ISerialize
	{
		void GetObjectData(XmlWriter wr);

		void AfterSerializedIn();
	}
}
