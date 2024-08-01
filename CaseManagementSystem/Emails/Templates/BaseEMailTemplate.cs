namespace CaseManagementSystem.Emails.Templates
{
    public abstract class BaseEMailTemplate
    {
        internal string footer
        {
            get =>
@"<a clicktracking= 'off' href='https://elvisapp.azurewebsites.net/'>Case Management system</a><br/><br/>
<b>ESA Risk Ltd</b> | 08405809<br/><br/>
[Postal address currently TBC for this]<br/><br/>
admin@debtabase.com | +44 (0)843 515 8686<br/><br/><br/>
<span style='font-size:9.0pt;font-family:""Arial"",sans-serif;color:silver;'>CONFIDENTIALITY: This email and its attachments are confidential to the intended recipient. They may not be used by, disclosed to, or copied in any way to, anyone other than the intended recipient. If this email is received in error, please contact ESA Risk on +44 (0)843 515 8686, provide details of the sender and the address to which it has been sent and then delete it. Opinions, conclusions and other statements and information in this message that do not relate to the official business of ESA Risk shall be understood as neither given nor endorsed by them.<br/>
VIRUSES: Although we have taken steps to ensure that this email and any attachments are free from any virus, it is your responsibility to check that they are actually virus-free. We do not accept any responsibility for viruses.
</span>";

        }
    }
}
