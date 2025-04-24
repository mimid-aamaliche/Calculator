using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace small_calculator
{



    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        enum OperationType
        {
            Plus,
            Minus,
            Multiply,
            division,
            module
        }

        string OperatorExist = "";
        string Prioritystring = "%/x+-";
        short PriorityIndex = 0;



        
        struct STCalculate
        {


            
            public float Num1 ;
            public float Num2;
            public int  StartIndex;
            public int EndIndex;
            public char Operation;
            public float Results;

        }
        STCalculate calculate = new STCalculate();


        float BetwenToNumbers(float Number1,float Number2,char Operation)
        {

            switch (Operation)
            {

                case '+':
                    return Number1+Number2;
                    break;

                case  '-' :
                    return Number1-Number2;
                    break;

                case 'x':
                    return Number1 * Number2;
                    break;

                case '/':
                    return Number1 / Number2;
                    break;

                case '%':
                    return Number1 % Number2;
                    break;

                default:
                    MessageBox.Show("problem in Betwen Tow Numbers Function Mate");
                    break;





            }





            return 0;
        }

        //in this function we will try to search for any ) or ( if there is any 
        short LookForSeperatorIndex(string S1,char seperator='(')
        {

            for(short i = 0; i < S1.Length; i++)
            {
                if (S1[i] == seperator)
                {
                    return i;
                }



            }







            return -1;
        }


        //in this function we are trying to determine how many different operations there is 
        bool IsThereMoreThenOneOperationType(string S1)
        {
            string Operators = "x-+%/";
            short Index = 0;





            for(short i=0;i<Operators.Length;i++)
            {
                Index = LookForSeperatorIndex(S1, Operators[i]);

                if (Index != (-1))
                {
                    OperatorExist += Operators[i];


                }




            }


            

           return (OperatorExist.Length>1)?true:false;




        }



        //1+2+6*(5%70+5) how to calculate the expression inside the()::::


        string subtractNumbersBeforAndAfterIndex(string s1,int index)
        {
            string Line = "";

            for(int i = index-1; i >= 0; i--)
            {
                if (s1[i] == 'x' || s1[i]=='-' || s1[i] == '+' || s1[i] == '/' || s1[i] == '%')
                {


                    Line += s1.Substring(i+1, index-i-1);
                    calculate.StartIndex = i+1;
                    break;
                }
                else if (i==0)
                {


                    Line += s1.Substring(0,index);
                    calculate.StartIndex = i;
                    break;

                }




            }
            for (int i = index+1 ; i < s1.Length; i++)
            {
                if (s1[i] == 'x' || s1[i] == '-' || s1[i] == '+' || s1[i] == '/' || s1[i] == '%')
                {


                    Line += s1.Substring(index,i-index );

                    calculate.EndIndex = i-1;

                    break;
                }
                
                else if(i==s1.Length-1)
                {

                    Line += s1.Substring(index, i+1-index);

                    calculate.EndIndex = i;

                    break;



                }




            }

            return Line;



        }

        string changeActuelExpression(string SmallS1)
        {

            for (int j = 0; j < Prioritystring.Length; j++)
            {





                for (short i = 0; i < SmallS1.Length; i++)
                {

                    if (SmallS1[i] == Prioritystring[j])
                    {
                        PriorityIndex = 0;

                        calculate = getNum1_2_Operation(subtractNumbersBeforAndAfterIndex(SmallS1, i));

                        SmallS1 = SmallS1.Remove(calculate.StartIndex, calculate.EndIndex - calculate.StartIndex +1);
                        SmallS1 = SmallS1.Insert(calculate.StartIndex, BetwenToNumbers(calculate.Num1, calculate.Num2, calculate.Operation).ToString());

                        return SmallS1;





                    }






                }


            }
            return SmallS1;


        }
        string GetThePriorityExpAndCalcule(string SmallS1)
        {
           

                
            SmallS1=changeActuelExpression(SmallS1);



            return SmallS1;

        }


        //in this function we seperate the small expression we got to tow nums and an operation
        bool IsDigite(char s)
        {
            for(short i = 0; i < 10; i++)
            {

                if (Convert.ToInt32(s) == i)
                    return true;



            }



            return false;

        }

        bool IsDigite(string s)
        {
            for(int i=0; i<s.Length; i++)
            {

                if (!char.IsDigit(s[i]) && s[i]!=',') return false;



            }

            return true;



        }

        
        STCalculate getNum1_2_Operation(string s1)
        {
            int Index = 0;
            for(Index=0;Index< s1.Length;Index++)
            {
                if (!char.IsDigit(s1[Index])) 
                {
                    if (s1[Index] != ',')
                        break;

                    

                    
                }






            }

            calculate.Num1 = Convert.ToSingle(s1.Substring(0, Index));

            calculate.Num2= Convert.ToSingle(s1.Substring(Index+1, s1.Length-Index-1));

            calculate.Operation = s1[Index];
            
            
            return calculate;



        }


        string CalculateExpressionAccordingtoPriority(string Expression)
        {

            //we can add a recursion here so if the expression has mor then one interval ( () )

            




            while (!IsDigite(Expression))
            {


                Expression = GetThePriorityExpAndCalcule(Expression);


            }

                return Expression;

        }



        string CalculateExpression(string FullExpression)
        {

            if(string.IsNullOrWhiteSpace(FullExpression))
                return "";

            short Index = LookForSeperatorIndex(FullExpression);
            string ExpressionBetwenParntaise = "";

            //here to check if there is any parentaise ( )
            while (Index != (-1))
            {

                 //we store the small exprassion betwen() and calculated later after removing it from our expression
                 //but we still have one problem , we can t put () inside a () or the programm will crash
                 //we can make a recursion for it tho but how ?

                 ExpressionBetwenParntaise = FullExpression.Substring(Index + 1, LookForSeperatorIndex(FullExpression, ')') - Index - 1);


                 FullExpression  = FullExpression.Remove(Index, LookForSeperatorIndex(FullExpression, ')') - Index+1 );

                 FullExpression= FullExpression.Insert(Index, CalculateExpressionAccordingtoPriority(ExpressionBetwenParntaise));

                 Index = LookForSeperatorIndex(FullExpression);


            }
           

            //when we get here all we have is a simple expresion without any ()

                FullExpression= CalculateExpressionAccordingtoPriority(FullExpression);



            

            //to check if we are done calculating or not
            if (IsDigite(FullExpression))
            {

                calculate.Results = Convert.ToSingle(FullExpression);
                return FullExpression;

            }
            else
            {
                FullExpression = CalculateExpressionAccordingtoPriority(FullExpression);


            }


            //i dont think we will ever get here hhhh
            return FullExpression;
            


        }




        void UpdateResultScreen()
        {
            TxtBResutlScreen.Text = CalculateExpression(TxtBScreen.Text);

        }

        void UpdateButtons(Button b)
        {
            
            TxtBScreen.Text += Convert.ToString(b.Tag);


        }




       

        private void btnNum_Click(object sender, EventArgs e)
        {
            UpdateButtons((Button)sender);
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {

            if (ExpressionIsValide(TxtBScreen.Text) && NumberOfParentaiseIsValide(TxtBScreen.Text))
            {

                           UpdateResultScreen();



            }
            else
            {
                MessageBox.Show(TxtBScreen.Text+" Is Not Valide Expression");

            }


        }

        private void btnClearScreen_Click(object sender, EventArgs e)
        {
            TxtBResutlScreen.Text = "";
            TxtBScreen.Text = "";


            calculate.Results = 0;
            calculate.StartIndex = 0;
            calculate.EndIndex = 0;
            calculate.Num1 = 0;
            calculate.Num2 = 0;
            calculate.Operation = ' ';


            



        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == false)
            {

              timer1.Enabled = true;


            }
            else
            {


                timer1.Enabled = false;
                LblTick.Visible = false;
            }


            //Reset all the info
            TxtBResutlScreen.Text = "";
            TxtBScreen.Text = "";


            calculate.Results = 0;
            calculate.StartIndex = 0;
            calculate.EndIndex = 0;
            calculate.Num1 = 0;
            calculate.Num2 = 0;
            calculate.Operation = ' ';




        }


        //the Timer needs a counter
        int Counter = 0;

        DateTime Clock=new DateTime();
        private void timer1_Tick(object sender, EventArgs e)
        {
            

            if (Counter % 2 == 0)
            {
                LblTick.Visible = false;
            }
            else
            {
                LblTick.Visible = true;
            }

            Clock = DateTime.Now;
            LblClock.Text = Clock.ToString("T");



            Counter++;
            Counter=(Counter > 400000000) ? 0 : Counter;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //because i want the timer to work as soon as the form loads 
            timer1.Enabled=true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if(TxtBScreen.Text.Length>0)
            TxtBScreen.Text=TxtBScreen.Text.Substring(0,TxtBScreen.Text.Length-1);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            TxtBScreen.Text += Convert.ToString(button2.Tag);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TxtBScreen.Text += Convert.ToString(button4.Tag);
        }


        //here we make sure the user entered a valid expresion
        //a valide expresssion contains secquince of numbers then operator


        bool ExpressionIsValide(string exp)
        {
            char PrevCase = exp[0];


            //firstcase and last case should always be a number 
            if (!char.IsDigit(exp[0])&&( !char.IsDigit(exp[exp.Length-1]) && exp[exp.Length - 1]!='(' && exp[exp.Length - 1] != ')'))
            {

                return false;

            }


            for(int i=1;i<exp.Length; i++)
            {
                if (!char.IsDigit(exp[i]) && exp[i] != '(' && exp[i]!=')')
                {
                    if (!char.IsDigit(PrevCase) && PrevCase != '(' && PrevCase != ')')
                    {

                        return false;

                    }



                }


                PrevCase = exp[i];



            }


            return true;


        }

        bool NumberOfParentaiseIsValide(string exp)
        {
            int index = 0;

            while(LookForSeperatorIndex(exp)!=-1)
            {
                index=LookForSeperatorIndex(exp);

                if (LookForSeperatorIndex(exp,')')!=(-1))
                {

                   exp= exp.Remove(index, 1);
                    exp=exp.Remove(LookForSeperatorIndex(exp, ')'), 1);

                }
                else
                {
                    return false;
                }







            }

            //if ( does not exist in the first place we should check for ) if it does exist then return false if not return true

            if (LookForSeperatorIndex(exp, ')') != (-1))
            {

                return false;

            }



            return true;


        }

        private void TxtBScreen_Validating(object sender, CancelEventArgs e)
        {


            if (!ExpressionIsValide(TxtBScreen.Text) || !NumberOfParentaiseIsValide(TxtBScreen.Text))
            {

                e.Cancel = true;
                TxtBScreen.Focus();
                errorProvider1.SetError(TxtBScreen, "Invalide expression  ");



            }
            
           else
            {
                e.Cancel = false;

                errorProvider1.SetError(TxtBScreen, " ");

            }





        }
    }
}
