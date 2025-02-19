using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ejercicio112
{
    public partial class Form1 : Form
    {
        //Declaración variables
        Random RndLlegaConsulta;
        Random RndLlegaUrgencia;
        Random RndFinAtencion;

        //Random rnd5 = new Random();
        Double timer, tiempoLlegadaConsulta, tiempoProximaConsulta, tiempoLlegadaUrgencia, tiempoProximaUrgencia, tiempoAtencion, tiempoFinAtencion, tiempoRestante;

        //Double[] tiempoLlegadaCons = new Double[100000];
        //Double[] tiempoLlegadaUrg = new Double[100000];
        //Double[] tiempoEsperaCons = new Double[100000];
        //Double[] tiempoEsperaUrg = new Double[100000];
        Dictionary<int, double> tiempoLlegadaCons = new Dictionary<int, double>();
        Dictionary<int, double> tiempoLlegadaUrg = new Dictionary<int, double>();
        Dictionary<int, double> tiempoEsperaCons = new Dictionary<int, double>(); //creo borrar
        Dictionary<int, double> tiempoEsperaUrg = new Dictionary<int, double>(); //creo borrar

        Double tiempoEsperaMaxCons, tiempoEsperaMaxUrg, tiempoEsperaMostrar;

        String ev, estadoMedico, estadoActual;
        String pacienteMaxEsperaCons, pacienteMaxEsperaUrg;
        Double rndC,rndU,rndEND;
        Double xMin;
        bool banderaUrg;
        Double esperaHastaAhora;
        Int32 cola, colaC, colaU, colaMaxC, colaMaxU, colaMax;
        Int32 numeroConsulta, numeroUrgencia, numeroPaciente, numeroFinConsulta, numeroFinUrgencia, numeroConsultaInterrumpida;
        Int32 numeroCorteCons;

        Int32 filaColaConsMax, filaColaUrgMax, filaColaMax;
        Int32 filaTiempoEsperaConsMax, filaTiempoEsperaUrgMax;

        public Form1()
        {
            
            RndLlegaConsulta = new Random(Guid.NewGuid().GetHashCode());
            RndLlegaUrgencia = new Random(Guid.NewGuid().GetHashCode());
            RndFinAtencion = new Random(Guid.NewGuid().GetHashCode());
            InitializeComponent();
            // Inicializar variables
            estadoMedico = "L";
            tiempoFinAtencion = 999999;
            timer = 0;
            banderaUrg = false;
            esperaHastaAhora = 0;
            numeroConsulta = 0;
            numeroUrgencia = 0;
            numeroPaciente = 0;
            numeroFinConsulta = 0;
            numeroFinUrgencia = 0;
            colaMax = 0;
            colaMaxC = 0;
            colaMaxU = 0;
            cola = 0;
            colaC = 0;
            colaU = 0;
            tiempoEsperaMaxCons = 0;
            tiempoEsperaMaxUrg = 0;
            filaColaConsMax = 0;
            filaColaUrgMax = 0;
            filaColaMax = 0;
            filaTiempoEsperaConsMax = 0;
            filaTiempoEsperaUrgMax = 0;
            tiempoProximaConsulta = 0;
            tiempoProximaUrgencia = 0;
            tiempoAtencion = 0;
            tiempoRestante = 0;
            tiempoEsperaMostrar = 0;
            pacienteMaxEsperaCons = string.Empty;
            pacienteMaxEsperaUrg = string.Empty;
        }


        //Toda la simulación junta hasta el final
        private void btnIniciar_Click(object sender, EventArgs e)
        {
            do
            {
                //CONSULTA
                if (tiempoProximaConsulta <= tiempoProximaUrgencia && tiempoProximaConsulta <= tiempoFinAtencion)
                {
                    timer = GeneradorNros.Truncar(tiempoProximaConsulta);
                    numeroConsulta++;
                    tiempoLlegadaCons[numeroConsulta] = timer;

                    rndC = RndLlegaConsulta.NextDouble();
                    tiempoLlegadaConsulta = GeneradorNros.Exponencial(Convert.ToDouble(txtMediaConsLl.Text), rndC); 
                    tiempoProximaConsulta = GeneradorNros.Truncar(timer + tiempoLlegadaConsulta);
                    ev = "Llegada Consulta";
                    //llega consulta y medico libre
                    if (estadoMedico == "L")
                    {
                        estadoMedico = "AC";
                        rndEND = RndFinAtencion.NextDouble();
                        tiempoAtencion = GeneradorNros.Uniforme(Convert.ToDouble(txtUnifConsA.Text), Convert.ToDouble(txtUnifConsB.Text), rndEND);
                        tiempoFinAtencion = GeneradorNros.Truncar(timer + tiempoAtencion);
                        //ev = "Llegada Consulta";

                        // !! Podría sacar el GeneradorNros.Truncar((timer / 60)) ya que no me pide explicitamente mostrar las horas
                        dataGridView1.Rows.Add(ev + " nº: "+ numeroConsulta.ToString(), GeneradorNros.Truncar((timer / 60)), timer, rndC, tiempoLlegadaConsulta, tiempoProximaConsulta, "", "", tiempoProximaUrgencia, estadoMedico, rndEND, tiempoAtencion, banderaUrg, tiempoFinAtencion, colaC, colaU, "", colaMaxC, colaMaxU, colaMax, tiempoEsperaCons, tiempoEsperaUrg, tiempoEsperaMaxCons,tiempoEsperaMaxUrg,contAtencUrg,contAtencTot);
                    }

                    // llega consulta y medico ocupado
                    else
                    {
                        //va a la cola, aumenta cola consulta y cola total
                        colaC++;
                        cola++;

                        //actualiza cola maxima consulta y cola total
                        if (colaC > colaMaxC)
                        {
                            colaMaxC = colaC;
                            filaColaConsMax = dataGridView1.Rows.Count - 1;
                        }
                        if (cola > colaMax)
                        {
                            colaMax = cola;
                            filaColaMax = dataGridView1.Rows.Count - 1;
                        }
                        //ev = "Llegada Consulta";
                        dataGridView1.Rows.Add(ev + " nº: " + numeroConsulta.ToString(), GeneradorNros.Truncar((timer / 60)), timer, rndC, tiempoLlegadaConsulta, tiempoProximaConsulta, "", "", tiempoProximaUrgencia, estadoMedico, "", "", tiempoFinAtencion,banderaUrg, colaC, colaU, "",colaMaxC, colaMaxU ,colaMax, tiempoEsperaCons, tiempoEsperaUrg, tiempoEsperaMaxCons, tiempoEsperaMaxUrg, contAtencUrg, contAtencTot);
                    }
                    
                    
                    continue;
                }

                //URGENCIA
                if (tiempoProximaUrgencia <= tiempoProximaConsulta && tiempoProximaUrgencia <= tiempoFinAtencion)
                {
                    timer = GeneradorNros.Truncar(tiempoProximaUrgencia);
                    numeroUrgencia++;
                    tiempoLlegadaUrg[numeroUrgencia] = timer;
                    rndU = RndLlegaUrgencia.NextDouble();
                    tiempoLlegadaUrgencia = GeneradorNros.Exponencial(Convert.ToDouble(txtMediaUrgLl.Text), rndU);
                    tiempoProximaUrgencia = GeneradorNros.Truncar(timer + tiempoLlegadaUrgencia);
                    ev = "Llegada Urgencia";

                    //llega urgencia y medico libre
                    if (estadoMedico == "L")
                    {
                        estadoMedico = "AU";
                        rndEND = RndFinAtencion.NextDouble();
                        tiempoAtencion = GeneradorNros.Uniforme(Convert.ToDouble(txtUnifUrgA.Text), Convert.ToDouble(txtUnifUrgB.Text), rndEND);
                        tiempoFinAtencion = GeneradorNros.Truncar(timer + tiempoAtencion);
                        dataGridView1.Rows.Add(ev + " nº: " + numeroUrgencia.ToString(), GeneradorNros.Truncar((timer / 60)), timer, "", "", tiempoProximaConsulta, rndU, tiempoLlegadaUrgencia, tiempoProximaUrgencia, estadoMedico, rndEND, tiempoAtencion, tiempoFinAtencion, banderaUrg, colaC, colaU, "", colaMaxC, colaMaxU, colaMax, tiempoEsperaCons, tiempoEsperaUrg, tiempoEsperaMaxCons, tiempoEsperaMaxUrg, contAtencUrg, contAtencTot);
                    }
                    

                    else
                    {
                        //Si llega urgencia y esta atendiendo consulta
                        if (estadoMedico == "AC")
                        {
                            estadoMedico = "AU";
                            numeroConsultaInterrumpida = numeroFinConsulta + 1; 
                            colaC++; //simbolico no real
                            cola++;

                            ////actualiza cola maxima consulta y cola maxima total
                            //if (colaC > colaMaxC)
                            //{
                            //    colaMaxC = colaC;
                            //    filaColaConsMax = dataGridView1.Rows.Count - 1;
                            //}
                            //if (cola > colaMax)
                            //{
                            //    colaMax = cola;
                            //    filaColaMax = dataGridView1.Rows.Count - 1;
                            //}

                            tiempoRestante = GeneradorNros.Truncar(tiempoFinAtencion - timer);
                            rndEND = RndFinAtencion.NextDouble();
                            tiempoAtencion = GeneradorNros.Uniforme(Convert.ToDouble(txtUnifUrgA.Text), Convert.ToDouble(txtUnifUrgB.Text), rndEND);
                            tiempoFinAtencion = GeneradorNros.Truncar(timer + tiempoAtencion);
                            tiempoLlegadaCons[numeroConsultaInterrumpida] = timer;
                            banderaUrg = true;

                            //numeroCorteCons = numeroFinConsulta + 1; // !!! ya No se usa pero controlar
                            ev = "Llegada Urgencia nº: " + numeroUrgencia.ToString() + " - (Corta Cons. nº: " + numeroConsultaInterrumpida.ToString() + ")";
                            dataGridView1.Rows.Add(ev, GeneradorNros.Truncar((timer / 60)), timer, "", "", tiempoProximaConsulta, rndU, tiempoLlegadaUrgencia, tiempoProximaUrgencia, estadoMedico, rndEND, tiempoAtencion, tiempoFinAtencion, banderaUrg, colaC, colaU, tiempoRestante,"", colaMaxC, colaMaxU, colaMax, tiempoEsperaCons, tiempoEsperaUrg, tiempoEsperaMaxCons, tiempoEsperaMaxUrg, contAtencUrg, contAtencTot);    
                        }
                        
                        //llega urgencia y está atendiendo otra urgencia
                        else
                        {
                            
                            colaU++;
                            cola++;

                            //actualiza cola maxima urgencia y cola total
                            if (colaU > colaMaxU)
                            {
                                colaMaxU = colaU;
                                filaColaUrgMax = dataGridView1.Rows.Count - 1;
                            }
                            if (cola > colaMax)
                            {
                                colaMax = cola;
                                filaColaMax = dataGridView1.Rows.Count - 1;
                            }
                            ev = "Llegada Urgencia";
                            dataGridView1.Rows.Add(ev + " nº: " + numeroUrgencia.ToString(), GeneradorNros.Truncar((timer / 60)), timer, "", "", tiempoProximaConsulta, rndU, tiempoLlegadaUrgencia, tiempoProximaUrgencia, estadoMedico, rndEND, tiempoAtencion, tiempoFinAtencion, banderaUrg, colaC, colaU, tiempoRestante, colaMaxC, colaMaxU, colaMax, tiempoEsperaCons, tiempoEsperaUrg, tiempoEsperaMaxCons, tiempoEsperaMaxUrg, contAtencUrg, contAtencTot);
                        }


                    }  
                    
                    
                    continue;
                }
                //FIN ATENCION
                if (tiempoFinAtencion <= tiempoProximaConsulta && tiempoFinAtencion <= tiempoProximaUrgencia)
                {
                    timer = GeneradorNros.Truncar(tiempoFinAtencion);
                    estadoActual = estadoMedico;
                    //double tiempoEspActual = 0;

                    //pregunto por el estado del medico para saber si es fin consulta o fin urgencia
                    if (estadoActual == "AC") //si atendia una consulta no podía haber urgencias en cola
                    {
                        //numeroFinConsulta++;
                        esperaHastaAhora += timer - tiempoLlegadaCons[numeroFinConsulta];
                        //tiempoEspActual = timer - tiempoLlegadaCons[numeroFinConsulta];


                        if (tiempoEsperaCons[numeroFinConsulta] > tiempoEsperaMaxCons)
                        {
                            tiempoEsperaMaxCons = tiempoEsperaCons[numeroFinConsulta];
                            pacienteMaxEsperaCons = "Paciente nº: " + numeroFinConsulta.ToString() + " consulta";
                            filaTiempoEsperaConsMax = dataGridView1.Rows.Count - 1; // para pintar valor
                        }
                        numeroFinConsulta++;
                    }
                    else
                    {
                        numeroFinUrgencia++;
                        tiempoEsperaUrg[numeroFinUrgencia] = timer - tiempoLlegadaUrg[numeroFinUrgencia];

                        if (tiempoEsperaUrg[numeroFinUrgencia] > tiempoEsperaMaxUrg)
                        {
                            tiempoEsperaMaxUrg = tiempoEsperaUrg[numeroFinUrgencia];
                            pacienteMaxEsperaUrg = "Paciente nº: " + numeroFinUrgencia.ToString() + " urgencia";
                            filaTiempoEsperaUrgMax = dataGridView1.Rows.Count - 1;
                        }
                    }
                    if (banderaUrg == false)
                    {


                        //fin atencion y hay cola para urgencias
                        if (colaU > 0)
                        {
                            colaU--;
                            cola--;

                            estadoMedico = "AU";
                            rndEND = RndFinAtencion.NextDouble();
                            tiempoAtencion = GeneradorNros.Uniforme(Convert.ToDouble(txtUnifUrgA.Text), Convert.ToDouble(txtUnifUrgB.Text), rndEND);
                            tiempoFinAtencion = GeneradorNros.Truncar(timer + tiempoAtencion);
                        }
                        else
                        {
                            //fin atencion y hay cola para consultas y no hay urgencias
                            if (colaC > 0)
                            {
                                colaC--;
                                cola--;

                                estadoMedico = "AC";
                                rndEND = RndFinAtencion.NextDouble();
                                tiempoAtencion = GeneradorNros.Uniforme(Convert.ToDouble(txtUnifConsA.Text), Convert.ToDouble(txtUnifConsB.Text), rndEND);
                                tiempoFinAtencion = GeneradorNros.Truncar(timer + tiempoAtencion);
                            }
                            //fin atencion y no hay pacientes en cola de ningun tipo
                            else
                            {
                                estadoMedico = "L";
                                tiempoFinAtencion = 999999;

                                if (estadoActual == "AC")
                                {
                                    ev = "Fin Atencion consulta nº: " + numeroFinConsulta.ToString();
                                    tiempoEsperaMostrar = GeneradorNros.Truncar(tiempoEsperaCons[numeroFinConsulta]);
                                }
                                else
                                {
                                    ev = "Fin Atencion urgencia nº: " + numeroFinUrgencia.ToString();
                                    tiempoEsperaMostrar = GeneradorNros.Truncar(tiempoEsperaUrg[numeroFinUrgencia]);
                                }
                                dataGridView1.Rows.Add(ev, timer, "", "", tiempoProximaConsulta, "", "", tiempoProximaUrgencia, estadoMedico, "", "", "", colaC, colaU, "", tiempoEsperaMostrar,GeneradorNros.Truncar((timer / 60)));
                                continue;
                            }
                        }
                    }
                    // fin atencion y había una consulta interrumpida
                    else
                    {
                        banderaUrg = false;
                        tiempoFinAtencion = GeneradorNros.Truncar(timer + tiempoRestante);
                        //tiempoEsperaMostrar = GeneradorNros.Truncar(tiempoEsperaUrg[numeroFinUrgencia]);
                        tiempoEsperaMostrar = GeneradorNros.Truncar(timer - tiempoLlegadaCons[numeroConsultaInterrumpida]+esperaHastaAhora);

                        ev = "Fin Urgencia nº: " + numeroFinUrgencia.ToString() + " y Retoma Consulta nº: " + numeroConsultaInterrumpida.ToString();
                        estadoMedico = "AC";
                        colaC--;
                        cola--;
                        dataGridView1.Rows.Add(ev, timer, "", "", tiempoProximaConsulta, "", "", tiempoProximaUrgencia, estadoMedico, "", tiempoRestante, tiempoFinAtencion, colaC, colaU, "", tiempoEsperaMostrar, GeneradorNros.Truncar((timer / 60)));
                        tiempoRestante = 0;
                        continue;
                    }

                    if (estadoActual == "AC")
                    {
                        tiempoEsperaMostrar = GeneradorNros.Truncar(tiempoEsperaCons[numeroFinConsulta]);
                        ev = "Fin Atencion consulta nº: " + numeroFinConsulta.ToString();
                    }
                    else
                    {
                        tiempoEsperaMostrar = GeneradorNros.Truncar(tiempoEsperaUrg[numeroFinUrgencia]);
                        ev = "Fin Atencion urgencia nº: " + numeroFinUrgencia.ToString();
                    }

                   // dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].Style.BackColor = Color.Cyan;
                    dataGridView1.Rows.Add(ev, timer, "", "", tiempoProximaConsulta, "", "", tiempoProximaUrgencia, estadoMedico, rndEND, tiempoAtencion, tiempoFinAtencion, colaC, colaU, "", tiempoEsperaMostrar,GeneradorNros.Truncar((timer / 60)));
                    
                    continue;
                }
            } while (timer < xMin);

            dataGridView1.Rows.RemoveAt(dataGridView1.Rows[dataGridView1.Rows.GetLastRow(0)].Index - 1);
            dataGridView1.Rows.Add("Fin de la simulacion", xMin, "", "", "", "", "", "", "", "", "", "", "", "", "", "", (xMin / 60));

            btnDeA1.Enabled = false;
            btnIniciar.Enabled = false;

            //calcula cantidad de pacientes de distinto tipo
            numeroPaciente = numeroFinConsulta + numeroFinUrgencia;

            txtCantPacientes.Text = numeroPaciente.ToString();
            txtCantConsultas.Text = numeroFinConsulta.ToString();
            txtCantUrgencias.Text = numeroFinUrgencia.ToString();
            txtPorcUrg.Text = GeneradorNros.Truncar((Convert.ToDouble(numeroFinUrgencia) / Convert.ToDouble(numeroPaciente)) * 100).ToString() + "%";

            //calcula cola maxima
            txtCola.Text = colaMax.ToString();
            txtColaC.Text = colaMaxC.ToString();
            txtColaU.Text = colaMaxU.ToString();

            //calcula Tiempos Esperas Maximos

            txtEsperaCons.Text = GeneradorNros.Truncar(tiempoEsperaMaxCons).ToString();
            txtEsperaUrg.Text = GeneradorNros.Truncar(tiempoEsperaMaxUrg).ToString();
            txtPacienteCons.Text = pacienteMaxEsperaCons;
            txtPacienteUrg.Text = pacienteMaxEsperaUrg;

            //Pinto celdas que contienen datos relevantes
            dataGridView1.Rows[filaColaConsMax].Cells[12].Style.BackColor = Color.Cyan;
            dataGridView1.Rows[filaColaUrgMax].Cells[13].Style.BackColor = Color.Cyan;

            dataGridView1.Rows[filaTiempoEsperaConsMax].Cells[15].Style.BackColor = Color.Red;
            dataGridView1.Rows[filaTiempoEsperaUrgMax].Cells[15].Style.BackColor = Color.Orange;

            dataGridView1.Rows[filaColaMax].Cells[12].Style.ForeColor = Color.Red;
            dataGridView1.Rows[filaColaMax].Cells[13].Style.ForeColor = Color.Red;


            MessageBox.Show("Fin de la simulación");
  

        }      

        //De a 1 evento por vez
        private void button1_Click(object sender, EventArgs e)
        {
            Double controlTiempo = 0;
            if (tiempoProximaConsulta <= tiempoProximaUrgencia && tiempoProximaConsulta <= tiempoFinAtencion)
                controlTiempo = tiempoProximaConsulta;
            if (tiempoProximaUrgencia <= tiempoProximaConsulta && tiempoProximaUrgencia <= tiempoFinAtencion)
                controlTiempo = tiempoProximaUrgencia;
            if (tiempoFinAtencion <= tiempoProximaConsulta && tiempoFinAtencion <= tiempoProximaUrgencia)
                controlTiempo = tiempoFinAtencion;

            if (controlTiempo >= xMin)
                timer = controlTiempo;

            if (timer < xMin)
            {

                
                //CONSULTA
                if (tiempoProximaConsulta <= tiempoProximaUrgencia && tiempoProximaConsulta <= tiempoFinAtencion)
                {
                    timer = GeneradorNros.Truncar(tiempoProximaConsulta);    
                    numeroConsulta++;
                    tiempoLlegadaCons[numeroConsulta] = timer;
                    rndC = RndLlegaConsulta.NextDouble();    
                    tiempoLlegadaConsulta = GeneradorNros.Exponencial(Convert.ToDouble(txtMediaConsLl.Text), rndC);
                    tiempoProximaConsulta = GeneradorNros.Truncar(timer + tiempoLlegadaConsulta);

                    //llega consulta y medico libre
                    if (estadoMedico == "L")
                    {
                        estadoMedico = "AC";
                        rndEND = RndFinAtencion.NextDouble();
                        tiempoAtencion = GeneradorNros.Uniforme(Convert.ToDouble(txtUnifConsA.Text), Convert.ToDouble(txtUnifConsB.Text), rndEND);
                        tiempoFinAtencion = GeneradorNros.Truncar(timer + tiempoAtencion);
                        ev = "Llegada Consulta";
                        dataGridView1.Rows.Add(ev + " nº: " + numeroConsulta.ToString(), timer, rndC, tiempoLlegadaConsulta, tiempoProximaConsulta, "", "", tiempoProximaUrgencia, estadoMedico, rndEND, tiempoAtencion, tiempoFinAtencion, colaC, colaU, "","",GeneradorNros.Truncar(timer / 60));
                    }

                    // llega consulta y medico ocupado
                    else
                    {
                        colaC++;
                        cola++;

                        //actualiza cola maxima consulta
                        if (colaC > colaMaxC)
                        {
                            colaMaxC = colaC;
                            filaColaConsMax = dataGridView1.Rows.Count - 1;
                        }

                        if (cola > colaMax)
                        {
                            colaMax = cola;
                            filaColaMax = dataGridView1.Rows.Count - 1;
                        }

                        ev = "Llegada Consulta";
                        dataGridView1.Rows.Add(ev + " nº: " + numeroConsulta.ToString(), timer, rndC, tiempoLlegadaConsulta, tiempoProximaConsulta, "", "", tiempoProximaUrgencia, estadoMedico, "", "", "", colaC, colaU, "","", GeneradorNros.Truncar((timer / 60)));
                    }


                    return;
                }

                //URGENCIA
                if (tiempoProximaUrgencia <= tiempoProximaConsulta && tiempoProximaUrgencia <= tiempoFinAtencion)
                {
                    timer = GeneradorNros.Truncar(tiempoProximaUrgencia);
                    numeroUrgencia++;
                    tiempoLlegadaUrg[numeroUrgencia] = timer;
                    rndU = RndLlegaUrgencia.NextDouble();
                    tiempoLlegadaUrgencia = GeneradorNros.Exponencial(Convert.ToDouble(txtMediaUrgLl.Text), rndU);
                    tiempoProximaUrgencia = GeneradorNros.Truncar(timer + tiempoLlegadaUrgencia);

                    //llega urgencia y medico libre
                    if (estadoMedico == "L")
                    {
                        estadoMedico = "AU";
                        rndEND = RndFinAtencion.NextDouble();
                        tiempoAtencion = GeneradorNros.Uniforme(Convert.ToDouble(txtUnifUrgA.Text), Convert.ToDouble(txtUnifUrgB.Text), rndEND);
                        tiempoFinAtencion = GeneradorNros.Truncar(timer + tiempoAtencion);
                        ev = "Llegada Urgencia";
                        dataGridView1.Rows.Add(ev + " nº: " + numeroUrgencia.ToString(), timer, "", "", tiempoProximaConsulta, rndU, tiempoLlegadaUrgencia, tiempoProximaUrgencia, estadoMedico, rndEND, tiempoAtencion, tiempoFinAtencion, colaC, colaU, "","", GeneradorNros.Truncar((timer / 60)));
                    }
                    else
                    {
                        //Si llega urgencia y esta atendiendo consulta
                        if (estadoMedico == "AC")
                        {
                            estadoMedico = "AU";
                            numeroConsultaInterrumpida = numeroFinConsulta + 1;
                            colaC++;
                            cola++;

                            //actualiza cola maxima consulta
                            if (colaC > colaMaxC)
                            {
                                colaMaxC = colaC;
                                filaColaConsMax = dataGridView1.Rows.Count - 1;
                            }

                            if (cola > colaMax)
                            {
                                colaMax = cola;
                                filaColaMax = dataGridView1.Rows.Count - 1;
                            }


                            tiempoRestante = GeneradorNros.Truncar(tiempoFinAtencion - timer);
                            rndEND = RndFinAtencion.NextDouble();
                            tiempoAtencion = GeneradorNros.Uniforme(Convert.ToDouble(txtUnifUrgA.Text), Convert.ToDouble(txtUnifUrgB.Text), rndEND);
                            tiempoFinAtencion = GeneradorNros.Truncar(timer + tiempoAtencion);

                            banderaUrg = true;

                            //numeroCorteCons = numeroFinConsulta + 1; ya no la uso pero controlar
                            ev = "Llegada Urgencia nº: " + numeroUrgencia.ToString() + " - (Corta Cons. nº: " + numeroConsultaInterrumpida.ToString() + ")";
                            dataGridView1.Rows.Add(ev, timer, "", "", tiempoProximaConsulta, rndU, tiempoLlegadaUrgencia, tiempoProximaUrgencia, estadoMedico, rndEND, tiempoAtencion, tiempoFinAtencion, colaC, colaU, tiempoRestante,"",GeneradorNros.Truncar((timer / 60))); 
                        }
                        //llega urgencia y está atendiendo otra urgencia
                        else
                        {

                            colaU++;
                            cola++;

                            //actualiza cola maxima urgencia
                            if (colaU > colaMaxU)
                            {
                                colaMaxU = colaU;
                                filaColaUrgMax = dataGridView1.Rows.Count - 1;
                            }
                            if (cola > colaMax)
                            {
                                colaMax = cola;
                                filaColaMax = dataGridView1.Rows.Count - 1;
                            }

                            ev = "Llegada Urgencia";
                            dataGridView1.Rows.Add(ev + " nº: " + numeroUrgencia.ToString(), timer, "", "", tiempoProximaConsulta, rndU, tiempoLlegadaUrgencia, tiempoProximaUrgencia, estadoMedico, "", "", tiempoFinAtencion, colaC, colaU, "","",GeneradorNros.Truncar((timer / 60)));    
                        }

                        
                    }


                    return;
                }
                //FIN DE ATENCION
                if (tiempoFinAtencion <= tiempoProximaConsulta && tiempoFinAtencion <= tiempoProximaUrgencia)
                {
                    timer = GeneradorNros.Truncar(tiempoFinAtencion);
                    estadoActual = estadoMedico;

                    //pregunto por el estado del medico para saber si es fin consulta o fin urgencia
                    if (estadoActual == "AC")
                    {
                        numeroFinConsulta++;
                        tiempoEsperaCons[numeroFinConsulta] = timer - tiempoLlegadaCons[numeroFinConsulta];

                        if (tiempoEsperaCons[numeroFinConsulta] > tiempoEsperaMaxCons)
                        {
                            tiempoEsperaMaxCons = tiempoEsperaCons[numeroFinConsulta];
                            pacienteMaxEsperaCons = "Paciente nº: " + numeroFinConsulta.ToString() + " consulta";
                            filaTiempoEsperaConsMax = dataGridView1.Rows.Count - 1;
                        }
                    }
                    else
                    {
                        numeroFinUrgencia++;
                        tiempoEsperaUrg[numeroFinUrgencia] = timer - tiempoLlegadaUrg[numeroFinUrgencia];

                        if (tiempoEsperaUrg[numeroFinUrgencia] > tiempoEsperaMaxUrg)
                        {
                            tiempoEsperaMaxUrg = tiempoEsperaUrg[numeroFinUrgencia];
                            pacienteMaxEsperaUrg = "Paciente nº: " + numeroFinUrgencia.ToString() + " urgencia";
                            filaTiempoEsperaUrgMax = dataGridView1.Rows.Count - 1;
                        }
                    }

                    if (banderaUrg == false)
                    {


                        //fin atencion y hay cola para urgencias
                        if (colaU > 0)
                        {
                            colaU--;
                            cola--;

                            estadoMedico = "AU";
                            rndEND = RndFinAtencion.NextDouble();
                            tiempoAtencion = GeneradorNros.Uniforme(Convert.ToDouble(txtUnifUrgA.Text), Convert.ToDouble(txtUnifUrgB.Text), rndEND);    
                            tiempoFinAtencion = GeneradorNros.Truncar(timer + tiempoAtencion);
                        }
                        else
                        {
                            //fin atencion y hay cola para consulta y no hay urgencias
                            if (colaC > 0)
                            {
                                colaC--;
                                cola--;

                                estadoMedico = "AC";
                                rndEND = RndFinAtencion.NextDouble();
                                tiempoAtencion = GeneradorNros.Uniforme(Convert.ToDouble(txtUnifConsA.Text), Convert.ToDouble(txtUnifConsB.Text), rndEND);
                                tiempoFinAtencion = GeneradorNros.Truncar(timer + tiempoAtencion);
                            }
                            //fin atencion y no hay pacientes en cola de ningun tipo
                            else
                            {
                                estadoMedico = "L";
                                tiempoFinAtencion = 999999;

                                if (estadoActual == "AC")
                                {
                                    ev = "Fin Atencion consulta nº: " + numeroFinConsulta.ToString();
                                    GeneradorNros.Truncar(tiempoEsperaMostrar = tiempoEsperaCons[numeroFinConsulta]);
                                }
                                else
                                {
                                    ev = "Fin Atencion urgencia nº: " + numeroFinUrgencia.ToString();
                                    GeneradorNros.Truncar(tiempoEsperaMostrar = tiempoEsperaUrg[numeroFinUrgencia]);
                                }

                                dataGridView1.Rows.Add(ev, timer, "", "", tiempoProximaConsulta, "", "", tiempoProximaUrgencia, estadoMedico, "", "", "", colaC, colaU, "", tiempoEsperaMostrar, GeneradorNros.Truncar((timer / 60)));
                                return;
                            }
                        }
                    }
                    // fin atencion urgencia y había una consulta interrumpida
                    else
                    {
                        banderaUrg = false;
                        tiempoFinAtencion = GeneradorNros.Truncar(timer + tiempoRestante);
                        ev = "Fin Urgencia nº: " + numeroFinUrgencia.ToString() + " y Retoma Consulta nº: " + numeroConsultaInterrumpida.ToString();
                        tiempoEsperaMostrar = GeneradorNros.Truncar(tiempoEsperaUrg[numeroFinUrgencia]);
                        estadoMedico = "AC";
                        colaC--;
                        cola--;
                        dataGridView1.Rows.Add(ev, timer, "", "", tiempoProximaConsulta, "", "", tiempoProximaUrgencia, estadoMedico, "", tiempoRestante, tiempoFinAtencion, colaC, colaU, "", tiempoEsperaMostrar,GeneradorNros.Truncar((timer / 60)));
                        tiempoRestante = 0;
                        return;
                    }

                    if (estadoActual == "AC")
                    {
                        ev = "Fin Atencion consulta nº: " + numeroFinConsulta.ToString();
                        tiempoEsperaMostrar = GeneradorNros.Truncar(tiempoEsperaCons[numeroFinConsulta]);
                    }
                    else
                    {
                        tiempoEsperaMostrar = GeneradorNros.Truncar(tiempoEsperaUrg[numeroFinUrgencia]);
                        ev = "Fin Atencion urgencia nº: " + numeroFinUrgencia.ToString();
                    }

                   // dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].Style.BackColor = Color.Cyan;
                    dataGridView1.Rows.Add(ev, timer, "", "", tiempoProximaConsulta, "", "", tiempoProximaUrgencia, estadoMedico, rndEND, tiempoAtencion, tiempoFinAtencion, colaC, colaU, "", tiempoEsperaMostrar, GeneradorNros.Truncar((timer / 60)));
                    
                    return;
                }
              
            }
            else
            {
                dataGridView1.Rows.Add("Fin de la simulacion", xMin, "", "", "", "", "", "", "", "", "", "", "", "", "", "", (xMin / 60));

                btnDeA1.Enabled = false;
                btnIniciar.Enabled = false;

                //calcula cantidad de pacientes de los distintos tipos
                numeroPaciente = numeroFinConsulta + numeroFinUrgencia;

                txtCantPacientes.Text = numeroPaciente.ToString();
                txtCantConsultas.Text = numeroFinConsulta.ToString();
                txtCantUrgencias.Text = numeroFinUrgencia.ToString();
                txtPorcUrg.Text = GeneradorNros.Truncar((Convert.ToDouble(numeroFinUrgencia) / Convert.ToDouble(numeroPaciente)) * 100).ToString() + "%";

                //calcula cola maxima
                txtCola.Text = colaMax.ToString();
                txtColaC.Text = colaMaxC.ToString();
                txtColaU.Text = colaMaxU.ToString();

                //calcula Tiempos Esperas Maximos

                txtEsperaCons.Text = GeneradorNros.Truncar(tiempoEsperaMaxCons).ToString();
                txtEsperaUrg.Text = GeneradorNros.Truncar(tiempoEsperaMaxUrg).ToString();
                txtPacienteCons.Text = pacienteMaxEsperaCons;
                txtPacienteUrg.Text = pacienteMaxEsperaUrg;

                //Pinto celdas que contienen datos relevantes
                dataGridView1.Rows[filaColaConsMax].Cells[12].Style.BackColor = Color.Cyan;
                dataGridView1.Rows[filaColaUrgMax].Cells[13].Style.BackColor = Color.Cyan;
                dataGridView1.Rows[filaTiempoEsperaConsMax].Cells[15].Style.BackColor = Color.Red;
                dataGridView1.Rows[filaTiempoEsperaUrgMax].Cells[15].Style.BackColor = Color.Orange;
                dataGridView1.Rows[filaColaMax].Cells[12].Style.ForeColor = Color.Red;
                dataGridView1.Rows[filaColaMax].Cells[13].Style.ForeColor = Color.Red;
                MessageBox.Show("Fin de la simulación");
            }
        }
       
    // GENERO PRIMERA LINEA
        private void button1_Click_1(object sender, EventArgs e)
        {
            bool validaNumeros;
            Int32 result;

            if (txtHoras.Text.Trim().Equals(""))
            {
                MessageBox.Show("Ingrese la cantidad de horas a simular");
                txtHoras.Focus();
                return;
            }
            if (txtMediaConsLl.Text.Trim().Equals(""))
            {
                MessageBox.Show("Ingrese el tiempo medio de llegada de pacientes a consulta");
                txtMediaConsLl.Focus();
                return;
            }

            if (txtMediaUrgLl.Text.Trim().Equals(""))
            {
                MessageBox.Show("Ingrese el tiempo medio de llegada de pacientes para urgencia");
                txtMediaUrgLl.Focus();
                return;
            }

            if (txtUnifUrgA.Text.Trim().Equals(""))
            {
                MessageBox.Show("Ingrese el valor de A para la distribución uniforme de las Urgencias");
                txtUnifUrgA.Focus();
                return;
            }

            if (txtUnifConsA.Text.Trim().Equals(""))
            {
                MessageBox.Show("Ingrese el valor de A para la distribución uniforme de las Consultas");
                txtUnifConsA.Focus();
                return;
            }

            if (txtUnifUrgB.Text.Trim().Equals(""))
            {
                MessageBox.Show("Ingrese el valor de B para la distribución uniforme de las Urgencia");
                txtUnifUrgB.Focus();
                return;
            }

            if (txtUnifConsB.Text.Trim().Equals(""))
            {
                MessageBox.Show("Ingrese el valor de B para la distribución uniforme de las Consultas");
                txtUnifConsB.Focus();
                return;
            }

            validaNumeros = Int32.TryParse(txtHoras.Text, out result);
            if (validaNumeros == false)
            {
                MessageBox.Show("Error en la cantidad de horas. Debe ser un entero.");
                txtHoras.Focus();
                return;
            }

            validaNumeros = Int32.TryParse(txtMediaConsLl.Text, out result);
            if (validaNumeros == false)
            {
                MessageBox.Show("Error en la media de llegada pacientes consulta. Debe ser un entero.");
                txtMediaConsLl.Focus();
                return;
            }

            validaNumeros = Int32.TryParse(txtMediaUrgLl.Text, out result);
            if (validaNumeros == false)
            {
                MessageBox.Show("Error en la media de llegada pacientes urgencia. Debe ser un entero.");
                txtMediaUrgLl.Focus();
                return;
            }

            validaNumeros = Int32.TryParse(txtUnifConsA.Text, out result);
            if (validaNumeros == false)
            {
                MessageBox.Show("Error en el rango tiempo atencion consulta (A). Debe ser un entero.");
                txtUnifConsA.Focus();
                return;
            }

            validaNumeros = Int32.TryParse(txtUnifUrgA.Text, out result);
            if (validaNumeros == false)
            {
                MessageBox.Show("Error en el rango tiempo atencion urgencia (A). Debe ser un entero.");
                txtUnifUrgA.Focus();
                return;
            }

            validaNumeros = Int32.TryParse(txtUnifConsB.Text, out result);
            if (validaNumeros == false)
            {
                MessageBox.Show("Error en el rango tiempo atencion consulta (B). Debe ser un entero.");
                txtUnifConsB.Focus();
                return;
            }

            validaNumeros = Int32.TryParse(txtUnifUrgB.Text, out result);
            if (validaNumeros == false)
            {
                MessageBox.Show("Error en el rango tiempo atencion urgencia (B). Debe ser un entero.");
                txtUnifUrgB.Focus();
                return;
            }


            if (Convert.ToInt32(txtUnifConsA.Text) > Convert.ToInt32(txtUnifConsB.Text))
            {
                MessageBox.Show("El rango de duración de consulta promedio es incorrecto");
                txtUnifConsA.Focus();
                return;
            }

            if (Convert.ToInt32(txtUnifUrgA.Text) > Convert.ToInt32(txtUnifUrgB.Text))
            {
                MessageBox.Show("El rango de duración de urgencia promedio es incorrecto");
                txtUnifUrgA.Focus();
                return;
            }

            btnDeA1.Enabled = true;
            btnIniciar.Enabled = true;
            button1.Enabled = false;

            txtMediaConsLl.ReadOnly = true;
            txtMediaUrgLl.ReadOnly = true;
            txtHoras.ReadOnly = true;
            txtUnifConsA.ReadOnly = true;
            txtUnifUrgA.ReadOnly = true;
            txtUnifConsB.ReadOnly = true;
            txtUnifUrgB.ReadOnly = true;

            dataGridView1.Rows.Clear();

            tiempoFinAtencion = 999999;
            estadoMedico = "L";
            timer = 0;
            banderaUrg = false;
            numeroConsulta = 0; numeroUrgencia = 0; numeroPaciente = 0; numeroFinConsulta = 0; numeroFinUrgencia=0;
            colaMax = 0; colaMaxC = 0; colaMaxU = 0;
            cola = 0;  colaC = 0; colaU = 0;
            tiempoEsperaMaxCons = 0;
            tiempoEsperaMaxUrg = 0;

            xMin = Convert.ToDouble(txtHoras.Text) * 60;
            //calculo tiempo entre llegadas y proxima llegada consulta
            rndC = RndLlegaConsulta.NextDouble();
            tiempoLlegadaConsulta = GeneradorNros.Exponencial(Convert.ToDouble(txtMediaConsLl.Text), rndC);
            tiempoProximaConsulta = timer + tiempoLlegadaConsulta;

            //calculo tiempo entre llegadas y proxima llegada urgencia
            rndU = RndLlegaUrgencia.NextDouble();
            tiempoLlegadaUrgencia = GeneradorNros.Exponencial(Convert.ToDouble(txtMediaUrgLl.Text), rndU);
            tiempoProximaUrgencia = timer + tiempoLlegadaUrgencia;

            ev = "Inicio de Simulación";

            dataGridView1.Rows.Add(ev, timer, rndC, tiempoLlegadaConsulta, tiempoProximaConsulta, rndU, tiempoLlegadaUrgencia, tiempoProximaUrgencia, "", "", "", "", "", "", "","", GeneradorNros.Truncar(timer / 60));
        }
    
    // RESETEAR
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            btnDeA1.Enabled = false;
            btnIniciar.Enabled = false;
            button1.Enabled = true;

            txtHoras.Text = "50";
            txtMediaConsLl.Text = "40";
            txtMediaUrgLl.Text = "105";

            txtUnifConsA.Text = "12";
            txtUnifUrgA.Text = "25";
            txtUnifConsB.Text = "20";
            txtUnifUrgB.Text = "40";

            txtMediaConsLl.ReadOnly = false;
            txtMediaUrgLl.ReadOnly = false;
            txtHoras.ReadOnly = false;
            txtUnifConsA.ReadOnly = false;
            txtUnifUrgA.ReadOnly = false;
            txtUnifConsB.ReadOnly = false;
            txtUnifUrgB.ReadOnly = false;

            txtCantConsultas.Text = "";
            txtCantPacientes.Text = "";
            txtCantUrgencias.Text = "";
            txtPorcUrg.Text = "";
            txtCola.Text = "";
            txtColaC.Text = "";
            txtColaU.Text = "";
            txtEsperaCons.Text = "";
            txtEsperaUrg.Text = "";
            txtPacienteCons.Text = "";
            txtPacienteUrg.Text = "";

        }

    // CARGA DE DATOS POR DEFECTO
        private void Form1_Load(object sender, EventArgs e)
        {
            txtHoras.Text = "50";
            txtMediaConsLl.Text = "40";
            txtMediaUrgLl.Text = "105";

            txtUnifConsA.Text = "12";
            txtUnifUrgA.Text = "25";
            txtUnifConsB.Text = "20";
            txtUnifUrgB.Text = "40";
        }

      
    }
}
