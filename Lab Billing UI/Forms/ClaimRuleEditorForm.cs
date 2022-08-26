using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MetroFramework.Forms;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using RFClassLibrary;
using System.Drawing;

namespace LabBilling.Forms
{
    public partial class ClaimRuleEditorForm : MetroForm
    {
        public ClaimRuleEditorForm()
        {
            InitializeComponent();
        }

        private List<ClaimValidationRule> claimRules;
        private List<string> propertyList;
        ClaimValidationRuleRepository claimRuleRepository = new ClaimValidationRuleRepository(Helper.ConnVal);

        private void ClaimRuleEditorForm_Load(object sender, EventArgs e)
        {
            cbOperator.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            cbOperator.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            cbMemberName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            cbMemberName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            cbLineType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            cbLineType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            cbMemberName.DropDownStyle = ComboBoxStyle.DropDown;

            listRules.View = View.Details;
            listRules.FullRowSelect = true;
            listRules.Columns.Add("RuleName", 200, HorizontalAlignment.Left);
            listRules.Columns.Add("RuleId", 10, HorizontalAlignment.Right);

            LoadRules();
        }

        private void LoadRules()
        {
            claimRules = claimRuleRepository.GetRules();
            listRules.Items.Clear();
            foreach (ClaimValidationRule rule in claimRules)
            {
                ListViewItem lvi = new ListViewItem(rule.ToString());
                lvi.SubItems.Add(rule.RuleId.ToString());

                listRules.Items.Add(lvi);
            }

        }

        private void listRules_SelectedIndexChanged(object sender, EventArgs e)
        {
            //load the tree view control with the rule
            if(listRules.SelectedItems.Count > 0)
            {
                tvRuleHierarchy.Nodes.Clear();
                int selectedRule = Convert.ToInt16(listRules.SelectedItems[0].SubItems[1].Text);

                ClaimValidationRule rule = claimRules.Where(r => r.RuleId == selectedRule).First<ClaimValidationRule>();

                TreeNode parentNode = new TreeNode(rule.RuleName.ToString())
                {
                    Tag = rule
                };

                var groups = rule.claimValidationRuleCriteria.Where(c => c.LineType == "Group");

                foreach (var group in groups)
                {
                    TreeNode groupNode = new TreeNode(group.ToString())
                    {
                        Tag = group
                    };

                    var details = rule.claimValidationRuleCriteria.Where(c => c.LineType == "Detail" && c.GroupId == group.GroupId);

                    foreach(var detail in details)
                    {
                        TreeNode detailNode = new TreeNode(detail.ToString())
                        {
                            Tag = detail
                        };

                        groupNode.Nodes.Add(detailNode);
                    }

                    var subGroups = rule.claimValidationRuleCriteria.Where(c => c.LineType == "SubGroup" && c.ParentGroupId == group.GroupId);

                    foreach (var subGroup in subGroups)
                    {
                        TreeNode subgroupNode = new TreeNode(subGroup.ToString())
                        {
                            Tag = subGroup
                        };

                        var sgDetails = rule.claimValidationRuleCriteria.Where(c => c.LineType == "Detail" && c.GroupId == subGroup.GroupId);
                        if (sgDetails != null)
                        {
                            foreach (var detail in sgDetails)
                            {
                                TreeNode detailNode = new TreeNode(detail.ToString())
                                {
                                    Tag = detail
                                };

                                subgroupNode.Nodes.Add(detailNode);
                            }
                        }
                        groupNode.Nodes.Add(subgroupNode);

                    }

                    parentNode.Nodes.Add(groupNode);
                }
                tvRuleHierarchy.Nodes.Add(parentNode);
                tvRuleHierarchy.ExpandAll();
            }
        }

        private void tvRuleHierarchy_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var rule = (ClaimValidationRule)tvRuleHierarchy.Nodes[0].Tag;

            tbErrorText.Text = rule.ErrorText;
            tbRuleName.Text = rule.RuleName;
            tbRuleDescription.Text = rule.Description;
            effectiveDate.Value = rule.EffectiveDate < effectiveDate.MinDate ? effectiveDate.MinDate : rule.EffectiveDate;
            endEffectiveDate.Value = rule.EndEffectiveDate > endEffectiveDate.MaxDate || rule.EndEffectiveDate < effectiveDate.MinDate ? endEffectiveDate.MaxDate : rule.EndEffectiveDate;
            if(tvRuleHierarchy.SelectedNode.Level == 0)
            {
                var header = rule.claimValidationRuleCriteria.Where(c => c.LineType == "Header").FirstOrDefault();
                if (header == null)
                {
                    header = new ClaimValidationRuleCriterion()
                    {
                        Operator = "AndAlso",
                        LineType = "Header"
                    };

                    rule.claimValidationRuleCriteria.Add(header);
                }
                cbLineType.SelectedItem = header.LineType;
                cbOperator.SelectedItem = header.Operator;
                tbTargetValue.Text = header.TargetValue;
            }
            if (tvRuleHierarchy.SelectedNode.Level > 0)
            {
                var detail = (ClaimValidationRuleCriterion)tvRuleHierarchy.SelectedNode.Tag;
                cbMemberName.SelectedItem = detail.MemberName;
                cbLineType.SelectedItem = detail.LineType;
                cbOperator.SelectedItem = detail.Operator;
                tbTargetValue.Text = detail.TargetValue;

                LoadMemberItems(detail.Class);

                cbMemberName.SelectedItem = detail.MemberName;
            }

        }

        private void LoadMemberItems(string itemClass)
        {
            if(string.IsNullOrEmpty(itemClass))
                throw new ArgumentNullException(nameof(itemClass));

            Type type = Type.GetType($"LabBilling.Core.Models.{itemClass},LabBilling Core");
            if (type != null)
                propertyList = ObjectProperties.GetProperties(type).ToList();

            propertyList.Sort();

            cbMemberName.Items.Clear();
            cbMemberName.Items.AddRange(propertyList.ToArray());

        }

        private void addRuleButton_Click(object sender, EventArgs e)
        {
            claimRules.Add(new ClaimValidationRule()
            {
                RuleId = 0,
                RuleName = "<new rule>",
                claimValidationRuleCriteria = new List<ClaimValidationRuleCriterion>()
            });

            ListViewItem lvi = new ListViewItem("<new rule>");
            lvi.SubItems.Add(0.ToString());
            listRules.Items.Add(lvi);
            //get index of newly created item
            int cnt = listRules.Items.Count;

            listRules.Items[cnt-1].Selected = true;
            
            listRules.Select();
            
        }

        private void saveRuleButton_Click(object sender, EventArgs e)
        {
            //loop through tree control and update/save nodes

            ClaimValidationRule rule = (ClaimValidationRule)tvRuleHierarchy.Nodes[0].Tag;

            rule.claimValidationRuleCriteria = new List<ClaimValidationRuleCriterion>();

            foreach(TreeNode node in tvRuleHierarchy.Nodes)
            {
                Queue<TreeNode> staging = new Queue<TreeNode>();
                staging.Enqueue(node);

                while(staging.Count > 0)
                {
                    TreeNode currentNode = staging.Dequeue();
                    if(currentNode.Tag.GetType() == typeof(ClaimValidationRuleCriterion))
                        rule.claimValidationRuleCriteria.Add((ClaimValidationRuleCriterion)currentNode.Tag);
                    foreach(TreeNode node2 in currentNode.Nodes)
                    {
                        staging.Enqueue(node2);
                    }
                }
            }

            claimRuleRepository.Save(rule);

            LoadRules();

        }

        //private List<ClaimValidationRuleCriterion> ReadRuleNodes(TreeNode node)
        //{
        //    List<ClaimValidationRuleCriterion> result = new List<ClaimValidationRuleCriterion>();

        //    result.Add((ClaimValidationRuleCriterion)node.Tag);

        //    foreach(TreeNode childNode in node.Nodes)
        //    {
        //        result.AddRange(ReadRuleNodes(childNode));
        //    }

        //    return result;
        //}

        private void removeDetailButton_Click(object sender, EventArgs e)
        {

        }

        private void addDetailButton_Click(object sender, EventArgs e)
        {
            var selectedNode = tvRuleHierarchy.SelectedNode;

            ClaimValidationRuleCriterion newCriterion = new ClaimValidationRuleCriterion();

            TreeNode newNode = new TreeNode();

            if (tvRuleHierarchy.SelectedNode.Level == 0) //this is the root node
            {
                ClaimValidationRule rule = (ClaimValidationRule)tvRuleHierarchy.SelectedNode.Tag;
                newCriterion.RuleId = rule.RuleId;
                newCriterion.Class = "Account";

                LoadMemberItems(newCriterion.Class);

                newNode.Tag = newCriterion;
                selectedNode.Nodes.Add(newNode);
            }
            else
            {
                //get parent node
                var selectedTag = (ClaimValidationRuleCriterion)tvRuleHierarchy.SelectedNode.Tag;

                newCriterion.RuleId = selectedTag.RuleId;
                newCriterion.ParentGroupId = selectedTag.ParentGroupId;
                newCriterion.GroupId = selectedTag.GroupId;
                newCriterion.Class = selectedTag.Class;

                //if selectedTag.MemberName property is a List<class> -this becomes of the class of the child node
                var memberNameType = ObjectProperties.GetProperty(Type.GetType($"LabBilling.Core.Models.{selectedTag.Class},LabBilling Core"), selectedTag.MemberName);
                if (memberNameType.Name == "List`1")
                {
                    //get the type from the property
                    Type genericType = memberNameType.GetGenericArguments()[0];
                    newCriterion.Class = genericType.Name;
                }


                LoadMemberItems(newCriterion.Class);

                newNode.Tag = newCriterion;

                if (selectedTag.LineType == "Group" || selectedTag.LineType == "SubGroup") // add a detail node
                {
                    selectedNode.Nodes.Add(newNode);
                }
                else if (selectedTag.LineType == "Detail") // add a detail node to parent node
                {
                    var parentNode = tvRuleHierarchy.SelectedNode.Parent;
                    parentNode.Nodes.Add(newNode);
                }
            }

            cbLineType.SelectedIndex = -1;
            cbMemberName.SelectedIndex = -1;
            cbOperator.SelectedIndex = -1;
            tbTargetValue.Text = String.Empty;

            tvRuleHierarchy.SelectedNode = newNode;
            newNode.EnsureVisible();

        }

        private void saveCriteraButton_Click(object sender, EventArgs e)
        {
            ClaimValidationRule rule = new ClaimValidationRule();
            ClaimValidationRuleCriterion detail = new ClaimValidationRuleCriterion();

            var selectedNode = tvRuleHierarchy.SelectedNode;
            if (selectedNode.Level == 0)
            {
                var selectedTag = (ClaimValidationRule)tvRuleHierarchy.SelectedNode.Tag;
                selectedTag.RuleName = tbRuleName.Text;
                selectedTag.Description = tbRuleDescription.Text;
                selectedTag.EffectiveDate = effectiveDate.Value;
                selectedTag.EndEffectiveDate = endEffectiveDate.Value;
                selectedTag.ErrorText = tbErrorText.Text;

                detail.RuleId = selectedTag.RuleId;
                detail.LineType = cbLineType.SelectedItem.ToString();
                detail.Operator = cbOperator.SelectedItem.ToString();
                selectedTag.claimValidationRuleCriteria.Add(detail);

                selectedNode.Text = tbRuleName.Text;
                selectedNode.Tag = selectedTag;
                listRules.SelectedItems[0].Text = tbRuleName.Text;
                
            }
            else
            {
                if(cbLineType.SelectedItem.ToString() == "Group")
                {
                    if(cbMemberName.SelectedIndex == -1)
                    {
                        MessageBox.Show("A group type detail line must have a valid Member selected.", "Missing Value", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        cbMemberName.BackColor = Color.Red;
                        return;
                    }
                }

                var selectedTag = (ClaimValidationRuleCriterion)tvRuleHierarchy.SelectedNode.Tag;
                selectedTag.TargetValue = tbTargetValue.Text;
                selectedTag.LineType = cbLineType.SelectedItem.ToString();
                selectedTag.MemberName = cbMemberName.SelectedItem != null ? cbMemberName.SelectedItem.ToString() : string.Empty;
                selectedTag.Operator = cbOperator.SelectedItem != null ? cbOperator.SelectedItem.ToString() : string.Empty;
                selectedNode.Tag = selectedTag;
                selectedNode.Text = selectedTag.ToString();
            }
        }

        private void resetSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if(menuItem != null)
            {
                ContextMenuStrip cbMenu = menuItem.Owner as ContextMenuStrip;
                if(cbMenu != null)
                {
                    if(cbMenu.SourceControl.GetType() == typeof(MetroFramework.Controls.MetroComboBox))
                    {
                        ((MetroFramework.Controls.MetroComboBox)cbMenu.SourceControl).SelectedIndex = -1;
                    }
                }
            }
        }
    }
}
