using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace Session_windows
{
	internal class Schema
	{
		static string x = @"<?xml version='1.0' encoding='Windows-1252'?>
		<xs:schema attributeFormDefault='unqualified' elementFormDefault='qualified' xmlns:xs='http://www.w3.org/2001/XMLSchema'>
		  <xs:element name='sessions'>
			<xs:complexType>
			  <xs:sequence>
				<xs:element name='excludedAppsList'>
				  <xs:complexType>
					<xs:sequence>
					  <xs:element minOccurs='0' maxOccurs='unbounded' name='excludedApp' type='xs:string' />
					</xs:sequence>
				  </xs:complexType>
				</xs:element>
				<xs:element maxOccurs='unbounded' name='session'>
				  <xs:complexType>
					<xs:sequence>
					  <xs:element maxOccurs='unbounded' name='process'>
						<xs:complexType>
						  <xs:simpleContent>
							<xs:extension base='xs:string'>
							  <xs:attribute name='winplacement' type='xs:unsignedByte' use='required' />
							  <xs:attribute name='xcoor' type='xs:short' use='required' />
							  <xs:attribute name='ycoor' type='xs:short' use='required' />
							  <xs:attribute name='width' type='xs:unsignedShort' use='required' />
							  <xs:attribute name='height' type='xs:unsignedShort' use='required' />
							</xs:extension>
						  </xs:simpleContent>
						</xs:complexType>
					  </xs:element>
					</xs:sequence>
					<xs:attribute name='name' type='xs:string' use='required' />
					<xs:attribute name='taskbarVisible' type='xs:string' use='required' />
				  </xs:complexType>
				</xs:element>
			  </xs:sequence>
			  <xs:attribute name='docked' type='xs:string' use='required' />
			  <xs:attribute name='undocked' type='xs:string' use='required' />
			  <xs:attribute name='startsystray' type='xs:unsignedByte' use='required' />
			</xs:complexType>
		  </xs:element>
		</xs:schema>";

		internal static XmlSchemaSet GetSchemaSet()
		{
			XmlSchemaSet set = new XmlSchemaSet();
			set.Add("", XmlReader.Create(new StringReader(x)));
			return set;
		}
	}
}
