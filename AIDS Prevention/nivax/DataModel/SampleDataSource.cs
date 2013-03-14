using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "Causes & Symptoms",
                    "Causes & Symptoms",
                    "Assets/Images/10.jpg",
                    "AIDS (acquired immune deficiency syndrome) is the final stage of HIV disease, which causes severe damage to the immune system.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Causes",
                    "AIDS is the sixth leading cause of death among people ages 25 - 44 in the United States, down from number one in 1995. Millions of people around the world are living with HIV/AIDS, including many children under age 15.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nAIDS is the sixth leading cause of death among people ages 25 - 44 in the United States, down from number one in 1995. Millions of people around the world are living with HIV/AIDS, including many children under age 15.\n\nHuman immunodeficiency virus (HIV) causes AIDS. The virus attacks the immune system and leaves the body vulnerable to a variety of life-threatening infections and cancers.\n\nCommon bacteria, yeast, parasites, and viruses that usually do not cause serious disease in people with healthy immune systems can cause fatal illnesses in people with AIDS.\n\nHIV has been found in saliva, tears, nervous system tissue and spinal fluid, blood, semen (including pre-seminal fluid, which is the liquid that comes out before ejaculation), vaginal fluid, and breast milk. However, only blood, semen, vaginal secretions, and breast milk have been shown to transmit infection to others.\n\nThe virus can be spread (transmitted):\nThrough sexual contact -- including oral, vaginal, and anal sex\nThrough blood -- via blood transfusions (now extremely rare in the U.S.) or needle sharing\nFrom mother to child -- a pregnant woman can transmit the virus to her fetus through their shared blood circulation, or a nursing mother can transmit it to her baby in her breast milk\nOther methods of spreading the virus are rare and include accidental needle injury, artificial insemination with infected donated semen, and organ transplantation with infected organs.\nHIV infection is NOT spread by:\nCasual contact such as hugging\nMosquitoes\nParticipation in sports\nTouching items that were touched by a person infected with the virus\nAIDS and blood or organ donation:\nAIDS is NOT transmitted to a person who DONATES blood or organs. People who donate organs are never in direct contact with people who receive them. Likewise, a person who donates blood is never in contact with the person receiving it. In all these procedures, sterile needles and instruments are used.\nHowever, HIV can be transmitted to a person RECEIVING blood or organs from an infected donor. To reduce this risk, blood banks and organ donor programs screen donors, blood, and tissues thoroughly.\nPeople at highest risk for getting HIV include:\nInjection drug users who share needles\nInfants born to mothers with HIV who didn't receive HIV therapy during pregnancy\nPeople engaging in unprotected sex, especially with people who have other high-risk behaviors, are HIV-positive, or have AIDS People who received blood transfusions or clotting products between 1977 and 1985 (before screening for the virus became standard practice)Sexual partners of those who participate in high-risk activities (such as injection drug use or anal sex)",
                    group1) { CreatedOn = "Group", CreatedTxt = "Causes & Symptoms", CreatedOnTwo = "Item", CreatedTxtTwo = "Causes", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/11.jpg")), CurrentStatus = "AIDS Prevention" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "Symptoms",
                     "AIDS begins with HIV infection. People who are infected with HIV may have no symptoms for 10 years or longer, but they can still transmit the infection to others during this symptom-free period. If the infection is not detected and treated, the immune system gradually weakens and AIDS develops.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nAIDS begins with HIV infection. People who are infected with HIV may have no symptoms for 10 years or longer, but they can still transmit the infection to others during this symptom-free period. If the infection is not detected and treated, the immune system gradually weakens and AIDS develops.\nAcute HIV infection progresses over time (usually a few weeks to months) to asymptomatic HIV infection (no symptoms) and then to early symptomatic HIV infection. Later, it progresses to AIDS (advanced HIV infection with CD4 T-cell count below 200 cells/mm3 ).\n\nAlmost all people infected with HIV, if they are not treated, will develop AIDS. There is a small group of patients who develop AIDS very slowly, or never at all. These patients are called nonprogressors, and many seem to have a genetic difference that prevents the virus from significantly damaging their immune system.\n\nThe symptoms of AIDS are mainly the result of infections that do not normally develop in people with a healthy immune system. These are called opportunistic infections.\n\nPeople with AIDS have had their immune system damaged by HIV and are very susceptible to these opportunistic infections. Common symptoms are:\nChills\nFever\nRash\nSweats (particularly at night)\nSwollen lymph glands\nWeakness\nWeight loss\n\nNote: At first, infection with HIV may produce no symptoms. Some people, however, do experience flu-like symptoms with fever, rash, sore throat, and swollen lymph nodes, usually 2 - 4 weeks after contracting the virus. This is called the acute retroviral syndrome. Some people with HIV infection stay symptom-free for years between the time when they are exposed to the virus and when they develop AIDS.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Causes & Symptoms", CreatedOnTwo = "Item", CreatedTxtTwo = "Symptoms", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/12.jpg")), CurrentStatus = "AIDS Prevention" });
            this.AllGroups.Add(group1);

            var group2 = new SampleDataGroup("Group-2",
                   "Exams & Tests",
                   "Exams & Tests",
                   "Assets/Images/20.jpg",
                   "The following is a list of AIDS-related infections and cancers that people with AIDS may get as their CD4 count decreases. In the past, having AIDS was defined as having HIV infection and getting one of these other diseases.");
            group2.Items.Add(new SampleDataItem("Group-2-Item-1",
                    "Genital Herpes",
                    "Genital herpes is a sexually transmitted viral infection affecting the skin or mucous membranes of the genitals.",
                    "Assets/DarkGray.png",
                    "",
                    "Details:\n\nGenital herpes is caused by two viruses:\n\nHerpes simplex virus type 2 (HSV-2)\nHerpes simplex virus type 1 (HSV-1)\nHerpes simplex virus type 2 (HSV-2) causes most cases of genital herpes. HSV-2 can be spread through secretions from the mouth or genitals.\nHerpes simplex virus type 1 (HSV-1) most often causes herpes infections of the mouth and lips (commonly called cold sores or fever blisters). HSV-1 can spread from the mouth to the genitals during oral sex.\nHerpes simplex virus (HSV) is spread from one person to another during sexual contact. You may be infected with herpes when your skin, vagina, penis, or mouth comes into contact with someone who already has herpes.\nHerpes is most likely to be transmitted by contact with the skin of an infected person who has visible sores, blisters, or a rash (an active outbreak), but you can also catch herpes from an infected person's skin when they have NO visible sores present (and the person may not even know that he or she is infected), or from an infected persons mouth (saliva) or vaginal fluids.Because the virus can be spread even when there are no symptoms or sores present, a sexual partner who has been infected with herpes in the past but has no active herpes sores can still pass the infection on to others.\nGenital HSV-2 infections is more common in women (approximately 1 of every 4 women is infected) than it is in men (nearly 1 of every 8 men is infected).",
                    group2) { CreatedOn = "Group", CreatedTxt = "Exams & Tests", CreatedOnTwo = "Item", CreatedTxtTwo = "Causes & Symptoms", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/21.jpg")), CurrentStatus = "AIDS Prevention" });
            group2.Items.Add(new SampleDataItem("Group-2-Item-2",
                     "Shingles",
                     "Shingles (herpes zoster) is a painful, blistering skin rash due to the varicella-zoster virus, the virus that causes chickenpox.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nAfter you get chickenpox, the virus remains inactive (becomes dormant) in certain nerves in the body. Shingles occurs after the virus becomes active again in these nerves years later.\nThe reason the virus suddenly becomes active again is not clear. Often only one attack occurs.\nShingles may develop in any age group, but you are more likely to develop the condition if:\nYou are older than 60\nYou had chickenpox before age 1\nYour immune system is weakened by medications or disease\nIf an adult or child has direct contact with the shingles rash and did not have chickenpox as a child or a chickenpox vaccine, they can develop chickenpox, not shingles.",
                     group2) { CreatedOn = "Group", CreatedTxt = "Exams & Tests", CreatedOnTwo = "Item", CreatedTxtTwo = "Shingles", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/22.jpg")), CurrentStatus = "AIDS Prevention" });
            group2.Items.Add(new SampleDataItem("Group-2-Item-3",
                      "Kaposi's Sarcoma",
                      "Kaposi's sarcoma is a cancerous tumor of the connective tissue, and is often associated with AIDS.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\nBefore the AIDS epidemic, Kaposi's sarcoma was seen mainly in elderly Italian and Jewish men, and rarely, in elderly women. Among this group, the tumors developed slowly. In AIDS patients, the cancer can develop quickly. The cancer may also involve the skin, lungs, gastrointestinal tract, and other organs.\n\nIn people with AIDS, Kaposi's sarcoma is caused by an interaction between HIV, a weakened immune system, and the human herpesvirus-8 (HHV-8). Kaposi's sarcoma has been linked to the spread of HIV and HHV-8 through sexual activity.\n\nPeople who have kidney or other organ transplants are also at risk for Kaposi's sarcoma.\n\nAfrican Kaposi's sarcoma is fairly common in young adult males living near the equator. One form is also common in young African children.",
                      group2) { CreatedOn = "Group", CreatedTxt = "Exams & Tests", CreatedOnTwo = "Item", CreatedTxtTwo = "Shingles", bgColour = "#20B2AA", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/23.jpg")), CurrentStatus = "AIDS Prevention" });
			group2.Items.Add(new SampleDataItem("Group-2-Item-4",
                      "Non-Hodgkin's Lymphoma",
                      "Non-Hodgkin's lymphoma is cancer of the lymphoid tissue, which includes the lymph nodes, spleen, and other organs of the immune system.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\nWhite blood cells called lymphocytes are found in lymph tissues. They help prevent infections. Most lymphomas start in a type of white blood cells called B lymphocytes, or B cells.\n\nFor most patients, the cause of this cancer is unknown. However, lymphomas may develop in people with weakened immune systems. For example, the risk of lymphoma increases after an organ transplant or in people with HIV infection.\n\nThere are many different types of non-Hodgkin's lymphoma (NHL). It is grouped according to how fast the cancer spreads.\n\nThe cancer may be low grade (slow growing), intermediate grade, or high grade (fast growing). Burkitt's lymphoma is an example of a high-grade lymphoma. Follicular lymphoma is a low-grade lymphoma\n\nThe cancer is further grouped by how the cells look under the microscope, for example, if there are certain proteins or genetic markers present.\n\nAccording to the American Cancer Society, a person has a 1 in 50 chance of developing non-Hodgkin's lymphoma. NHL most often affects adults. However, children can get some forms of lymphoma. You are more likely to get lymphoma if you have a weakened immune system or have had an organ transplant.\n\nThis type of cancer is slightly more common in men than in women.",
                      group2) { CreatedOn = "Group", CreatedTxt = "Exams & Tests", CreatedOnTwo = "Item", CreatedTxtTwo = "Shingles", bgColour = "#20B2AA", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/24.jpg")), CurrentStatus = "AIDS Prevention" });	
				group2.Items.Add(new SampleDataItem("Group-2-Item-5",
                      "Pneumocystis Jiroveci Pneumonia",
                      "Pneumocystis jiroveci pneumonia is a fungal infection of the lungs. The disease used to be called Pneumocystis carinii.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\nThis type of pneumonia is caused by the fungus Pneumocystis jiroveci . This fungus is common in the environment and does not cause illness in healthy people.\n\nHowever, it can cause a lung infection in people with a weakened immune system due to:\n\nCancer\n\nChronic use of corticosteroids or other medications that weaken the immune system\n\nHIV/AIDS\nOrgan or bone marrow transplant\n\nPneumocystis jiroveci was a relatively rare infection before the AIDS epidemic. Before the use of preventive antibiotics for the condition, most people in the United States with advanced AIDS would develop it.",
                      group2) { CreatedOn = "Group", CreatedTxt = "Exams & Tests", CreatedOnTwo = "Item", CreatedTxtTwo = "Shingles", bgColour = "#20B2AA", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/25.jpg")), CurrentStatus = "AIDS Prevention" });
				group2.Items.Add(new SampleDataItem("Group-2-Item-6",
                      "Meningitis - Cryptococcal",
                      "Cryptococcal meningitis is a fungal infection of the membranes covering the brain and spinal cord (meninges).",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\nCryptococcal meningitis is caused by the fungus Cryptococcus neoformans . This fungus is found in soil around the world.\n\nCryptococcal meningitis most often affects people with a weakened immune system. Risk factors include:\nAIDS\nDiabetes\nLymphoma\nIn people with a normal immune system and no chronic illnesses, it is a rare condition.",
                      group2) { CreatedOn = "Group", CreatedTxt = "Exams & Tests", CreatedOnTwo = "Item", CreatedTxtTwo = "Shingles", bgColour = "#20B2AA", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/26.jpg")), CurrentStatus = "AIDS Prevention" });
				group2.Items.Add(new SampleDataItem("Group-2-Item-7",
                      "Progressive Multifocal Leukoencephalopathy",
                      "Progressive multifocal leukoencephalopathy (PML) is a rare disorder that damages the material (myelin) that covers and protects nerves in the white matter of the brain.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\nThe JC virus (JCV) causes PML. By age 10, most people have been infected with this virus, but it hardly ever causes symptoms.\n\nAnyone with a weakened immune system, however, are at greater risk of developing PML. Causes of a weakened immune system include:\nAIDS (less common now because of better AIDS treatments)\nCertain medications used to treat multiple sclerosis, rheumatoid arthritis, and related conditions\nLeukemia and lymphoma",
                      group2) { CreatedOn = "Group", CreatedTxt = "Exams & Tests", CreatedOnTwo = "Item", CreatedTxtTwo = "Leukoencephalopathy", bgColour = "#20B2AA", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/27.jpg")), CurrentStatus = "AIDS Prevention" });
				group2.Items.Add(new SampleDataItem("Group-2-Item-8",
                      "Acute Cytomegalovirus (CMV) Infection",
                      "Acute cytomegalovirus (CMV) infection is a condition caused by a member of the herpesvirus family.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\nInfection with cytomegalovirus (CMV) is very common. The infection is spread by:\nBlood transfusions\nOrgan transplants\nRespiratory droplets\nSaliva\nSexual contact\nUrine\nMost people are exposed to CMV in their lifetime, but typically only individuals with weakened immune systems become ill from CMV infection. Some people with this condition develop a mononucleosis-like syndrome.\n\nIn the U.S., CMV infection most commonly develops between ages 10 - 35. Most people are exposed to CMV early in life and do not realize it because they have no symptoms. People with a compromised immune system can have a more severe form of the disease.",
                      group2) { CreatedOn = "Group", CreatedTxt = "Exams & Tests", CreatedOnTwo = "Item", CreatedTxtTwo = "CMV", bgColour = "#20B2AA", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/28.jpg")), CurrentStatus = "AIDS Prevention" });
            this.AllGroups.Add(group2);
			
            var group3 = new SampleDataGroup("Group-3",
                   "Treatment",
                   "Treatment",
                   "Assets/Images/30.jpg",
                   "There is no cure for AIDS at this time. However, a variety of treatments are available that can help keep symptoms at bay and improve the quality and length of life for those who have already developed symptoms.");
            group3.Items.Add(new SampleDataItem("Group-3-Item-1",
                    "HAART",
                    "Treatment with HAART has complications. HAART is a collection of different medications, each with its own side effects.",
                    "Assets/DarkGray.png",
                    "",
                    "Details:\n\nThere is no cure for AIDS at this time. However, a variety of treatments are available that can help keep symptoms at bay and improve the quality and length of life for those who have already developed symptoms.\n\nAntiretroviral therapy suppresses the replication of the HIV virus in the body. A combination of several antiretroviral drugs, called highly active antiretroviral therapy (HAART), has been very effective in reducing the number of HIV particles in the bloodstream. This is measured by the viral load (how much free virus is found in the blood). Preventing the virus from replicating can improve T-cell counts and help the immune system recover from the HIV infection.\n\nHAART is not a cure for HIV, but it has been very effective for the past 12 years. People on HAART with suppressed levels of HIV can still transmit the virus to others through sex or by sharing needles. There is good evidence that if the levels of HIV remain suppressed and the CD4 count remains high (above 200 cells/mm3), life can be significantly prolonged and improved.\n\nHowever, HIV may become resistant to one combination of HAART, especially in patients who do not take their medications on schedule every day. Genetic tests are now available to determine whether an HIV strain is resistant to a particular drug. This information may be useful in determining the best drug combination for each person, and adjusting the drug regimen if it starts to fail. These tests should be performed any time a treatment strategy begins to fail, and before starting therapy.\n\nWhen HIV becomes resistant to HAART, other drug combinations must be used to try to suppress the resistant strain of HIV. There are a variety of new drugs on the market for treating drug-resistant HIV.",
                    group3) { CreatedOn = "Group", CreatedTxt = "Treatment", CreatedOnTwo = "Item", CreatedTxtTwo = "HAART", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/31.jpg")), CurrentStatus = "AIDS Prevention" });
            group3.Items.Add(new SampleDataItem("Group-3-Item-2",
                     "Applied Methodology",
                     "When used for a long time, these medications increase the risk of heart attack, perhaps by increasing the levels of cholesterol and glucose (sugar) in the blood.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nWhen used for a long time, these medications increase the risk of heart attack, perhaps by increasing the levels of cholesterol and glucose (sugar) in the blood.\n\nAny doctor prescribing HAART should carefully watch the patient for possible side effects. In addition, blood tests measuring CD4 counts and HIV viral load should be taken every 3 months. The goal is to get the CD4 count as close to normal as possible, and to suppress the amount of HIV virus in the blood to a level where it cannot be detected.\n\nOther antiviral medications are being investigated. In addition, growth factors that stimulate cell growth, such as erthythropoetin (Epogen, Procrit, and Recomon) and filgrastim (G-CSF or Neupogen) are sometimes used to treat AIDS-associated anemia and low white blood cell counts.\n\nMedications are also used to prevent opportunistic infections (such as Pneumocystis jiroveci pneumonia) if the CD4 count is low enough. This keeps AIDS patients healthier for longer periods of time. Opportunistic infections are treated when they happen.",
                     group3) { CreatedOn = "Group", CreatedTxt = "Treatment", CreatedOnTwo = "Item", CreatedTxtTwo = "Applied Methodology", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/32.jpg")), CurrentStatus = "AIDS Prevention" });
            this.AllGroups.Add(group3);
			
			
         
        }
    }
}
