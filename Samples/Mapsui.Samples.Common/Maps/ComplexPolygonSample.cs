﻿using System.Collections.Generic;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.UI;
using Mapsui.Utilities;

namespace Mapsui.Samples.Common.Maps
{
    public class ComplexPolygonSample : ISample
    {
        public string Name => "4 Complex Polygons";

        public string Category => "Geometries";

        public void Setup(IMapControl mapControl)
        {
            mapControl.Map = CreateMap();
        }

        public static Map CreateMap()
        {
            var map = new Map();
            map.Layers.Add(OpenStreetMap.CreateTileLayer());
            var polygonLayer = CreateLayer();
            map.Layers.Add(polygonLayer);
            map.Home = n => n.NavigateTo(polygonLayer.Envelope.Centroid, map.Resolutions[15]);
            return map;
        }

        public static ILayer CreateLayer()
        {
            return new Layer("Polygons")
            {
                DataSource = new MemoryProvider(CreatePolygon()),
                Style = new VectorStyle
                {
                    Fill = new Brush(new Color(150, 150, 30, 128)),
                    Outline = new Pen
                    {
                        Color = Color.Orange,
                        Width = 2,
                        PenStyle = PenStyle.Solid,
                        PenStrokeCap = PenStrokeCap.Round
                    }
                }
            };
        }

        private static List<Polygon> CreatePolygon()
        {
            var result = new List<Polygon>();

            // Fails
            var polygon = (Polygon)Geometry.GeomFromText(@"POLYGON((601460.84271555 6161723.18906461, 601460.99114075 6161722.05929457, 601462.68978467 6161718.29006179, 601468.58556334 6161712.2112991, 601476.17998594 6161706.73241431, 601484.83812245 6161702.30331584, 601506.96996854 6161687.97623208, 601532.15277702 6161670.56977511, 601555.30710781 6161653.55323878, 601578.85723911 6161632.37754903, 601627.0129698 6161570.66011144, 601643.94168814 6161548.97452548, 601649.23552018 6161535.4772728, 601651.87419036 6161531.43809496, 601683.36506972 6161483.57783678, 601704.45793943 6161452.24421465, 601726.95260267 6161440.3366384, 601735.31388879 6161435.74757249, 601760.43897636 6161415.03178913, 601783.67576558 6161396.05565167, 601801.288889 6161374.61001687, 601802.79787851 6161333.37840944, 601801.26415147 6161320.62100617, 601796.21769476 6161291.11701163, 601793.36463263 6161266.99192222, 601791.49282598 6161254.38448843, 601790.33016193 6161242.80684502, 601789.27469386 6161228.53974905, 601789.52206919 6161216.13227455, 601789.55505257 6161211.12329412, 601788.53256787 6161206.80417326, 601786.31443576 6161196.77621442, 601782.45538063 6161180.33956006, 601779.20651798 6161168.95187799, 601779.71776032 6161158.03410027, 601780.58357397 6161147.12632051, 601786.41338589 6161132.25934664, 601791.55054689 6161123.87105406, 601799.3346239 6161115.48276147, 601808.61944458 6161107.61436306, 601819.61940087 6161099.4460257, 601827.37874035 6161093.92714905, 601833.71979462 6161088.4082724, 601842.17178502 6161079.84001645, 601852.91612014 6161066.60271086, 601863.6687011 6161053.39539917, 601872.29385423 6161044.83714118, 601884.6296373 6161035.92895442, 601895.71205203 6161030.30010015, 601907.14079223 6161023.97138835, 601925.47954994 6161015.18317716, 601938.43377133 6161009.16440227, 601947.32279148 6161002.965664, 601954.232809 6160992.17785983, 601960.07086677 6160982.17989488, 601969.04234536 6160965.51328732, 601973.07456322 6160954.84545873, 601972.29120801 6160942.15804122, 601969.21550809 6160928.24087402, 601966.73350896 6160919.44266487, 601962.14057368 6160914.53366408, 601957.57237594 6160910.55447403, 601955.8325028 6160907.27514153, 601955.71706098 6160897.76707687, 601957.58062179 6160889.10883923, 601958.04238907 6160881.86031464, 601957.24200669 6160873.93192844, 601957.85273465 6160866.00354224, 601957.93519309 6160854.23593751, 601960.82948444 6160846.48751468, 601962.31373641 6160840.68869501, 601968.81146172 6160827.67134465, 601976.03482132 6160814.58400854, 601982.54903832 6160802.11654626, 601984.47856588 6160796.50768792, 601985.62473824 6160791.22876243, 601988.17270413 6160788.77926102, 601990.55575313 6160786.53971688, 601996.3773192 6160782.98044135, 602004.3263131 6160779.25120043, 602015.30153186 6160775.25201445, 602023.6298346 6160771.43279185, 602032.01585825 6160764.67416754, 602040.41012774 6160754.74618836, 602046.9738198 6160745.14814201, 602053.90032901 6160736.25995118, 602058.97152325 6160731.96082625, 602065.09818556 6160729.06141642, 602071.66187762 6160728.79147136, 602075.57865366 6160729.94123733, 602082.25778754 6160735.00020759, 602087.19704828 6160738.8194302, 602088.41743323 6160741.13895807, 602084.17082342 6160743.81841267, 602077.59063967 6160748.09754167, 602071.71135269 6160754.49623923, 602067.25035093 6160764.31424081, 602062.81408669 6160778.11143243, 602054.79088019 6160795.87781613, 602047.80665008 6160808.05533743, 602042.58703064 6160821.29264302, 602037.28495275 6160836.2995884, 602036.69949781 6160842.93823712, 602035.40490025 6160852.56627736, 602033.26922658 6160862.38427893, 602030.06983899 6160876.43141968, 602024.59459838 6160893.54793566, 602020.52939714 6160907.14516799, 602017.70107255 6160919.0627422, 602015.92821603 6160932.04010069, 602011.30229738 6160944.26761182, 602007.03095003 6160957.7348706, 602004.2521005 6160962.53389377, 602001.74536384 6160970.69223316, 602000.68989577 6160981.46004141, 601999.3375773 6160990.25825056, 601998.53773041 6160999.38639255, 602000.91253356 6161008.34456914, 602000.57445395 6161014.71327281, 601999.18090626 6161021.07197851, 601993.88707422 6161037.94854334, 601991.2401582 6161047.61657544, 601989.64871026 6161056.93467876, 601988.23042504 6161068.59230589, 601987.85936204 6161081.65964607, 601988.18919582 6161104.60497561, 601989.28589311 6161115.32279403, 601991.08348716 6161120.45175005, 601993.58197798 6161125.92063687, 601998.23263417 6161131.93941177, 601999.96426147 6161135.0387809, 602000.5662081 6161138.3481073, 602002.58643996 6161139.18793635, 602004.4005257 6161146.01654641, 602007.08867094 6161157.254259, 602005.79407339 6161158.6239802, 602003.26259919 6161156.114491, 601998.36456768 6161155.57460089, 601995.222901 6161157.86413486, 601991.02576625 6161162.29323333, 601988.16445828 6161165.29262282, 601985.65772162 6161169.35179658, 601983.53853963 6161177.7600851, 601981.77392895 6161185.29855067, 601979.0280628 6161188.61787503, 601977.86539876 6161191.05737847, 601977.31292719 6161197.6960272, 601974.78145299 6161203.56483262, 601967.26124299 6161225.89028834, 601962.10759031 6161243.58668629, 601960.05437508 6161255.54425236, 601959.06487376 6161267.86174517, 601959.98016248 6161279.21943334, 601962.94042058 6161283.63853385, 601967.13755533 6161287.34777884, 601972.86841711 6161289.40735962, 601976.44711354 6161289.27738608, 601982.31815468 6161290.74708692, 601990.21767351 6161292.91664532, 601997.23488701 6161294.02641942, 602011.269314 6161296.96582112, 602016.1096246 6161297.97561558, 602023.42368849 6161299.87522892, 602034.86067453 6161305.43409743, 602046.09151446 6161311.22291913, 602056.06074022 6161314.74220279, 602067.26684261 6161319.31127277, 602079.1408584 6161322.60060323, 602083.34623899 6161323.5504099, 602091.43541225 6161324.51021454, 602102.66625218 6161328.52939644, 602114.30938432 6161331.29883273, 602128.67364509 6161332.55857632, 602137.90074485 6161333.16845218, 602149.04912634 6161335.93788847, 602160.26347459 6161337.44758117, 602170.94184295 6161337.8974896, 602178.58574061 6161337.62754454, 602181.34809845 6161336.43778671, 602183.40131368 6161337.00767072, 602192.55420085 6161342.6865148, 602202.06165932 6161348.71528766, 602207.32250798 6161352.66448382, 602212.58335664 6161358.71325261, 602216.04661125 6161362.18254644, 602221.95063576 6161371.48065384, 602225.43038206 6161378.68918656, 602231.38388164 6161391.0266753, 602240.24816426 6161405.18379366, 602251.41303743 6161419.88080213, 602260.2855659 6161431.2384903, 602266.55240756 6161433.88795101, 602274.58385991 6161436.53741172, 602292.41137527 6161441.1364756, 602297.12799821 6161456.1934108, 602298.40610408 6161463.96182956, 602296.16323443 6161467.49111119, 602293.3101723 6161473.97979044, 602288.87390807 6161483.10793243, 602284.429398 6161493.11589534, 602280.53735949 6161497.65497142, 602274.69930173 6161504.84350822, 602267.26155017 6161512.16201856, 602257.71286247 6161518.58071205, 602244.63495342 6161526.39912063, 602236.50455095 6161531.0581723, 602233.66798051 6161533.06776325, 602228.67924471 6161533.12775104, 602211.51139689 6161533.00777546, 602207.52865409 6161533.83760655, 602205.5578973 6161534.76741729, 602202.2430679 6161532.93778971, 602196.28956831 6161529.6884511, 602185.34733293 6161525.82923663, 602175.81513693 6161522.66987971, 602170.91710542 6161522.02001199, 602163.55356646 6161522.42992855, 602148.12559178 6161523.59969045, 602124.31159345 6161525.59928344, 602106.03055665 6161525.0393974, 602096.17677272 6161525.98920407, 602089.33272195 6161527.28893951, 602086.10035099 6161526.21915727, 602081.11161519 6161523.90962736, 602078.57189514 6161522.19997536, 602075.32303249 6161515.21139786, 602070.5404428 6161503.98368323, 602067.52246379 6161501.94409838, 602061.6431768 6161490.54641835, 602054.35385045 6161481.62823362, 602049.26616452 6161474.10976398, 602043.82390728 6161468.0010074, 602037.65601575 6161462.44213889, 602022.31049951 6161454.06384427, 602020.90870598 6161452.96406813, 602019.85323791 6161449.74472341, 602015.63136563 6161448.25502664, 601989.24466389 6161441.94631076, 601968.29197353 6161438.44702303, 601964.62257282 6161437.75716345, 601962.02513186 6161437.91713089, 601945.13764275 6161442.3062375, 601927.07099789 6161446.47538888, 601907.94063913 6161452.73411493, 601888.81028036 6161457.62311979, 601877.05170639 6161463.29196591, 601862.47305368 6161471.0803806, 601851.47309739 6161477.53906596, 601850.0053371 6161479.7786101, 601849.09004839 6161483.07793853, 601846.41014899 6161485.31748268, 601838.58484275 6161492.96592586, 601834.93193373 6161494.16568166, 601834.08261177 6161495.42542524, 601832.72204746 6161500.66435887, 601831.00691184 6161507.31300556, 601830.40496521 6161513.90166446, 601828.13735803 6161523.20976982, 601822.60439651 6161538.24670909, 601822.21684182 6161541.60602531, 601820.60065634 6161543.89555928, 601820.18011828 6161546.7149854, 601821.80454961 6161549.18448274, 601822.77755924 6161550.77415917, 601823.92373159 6161553.56359138, 601823.93197744 6161560.6021587, 601827.84875348 6161568.87047571, 601827.49418217 6161575.75907356, 601826.05115942 6161580.37813336, 601823.37950587 6161583.94740685, 601820.62539387 6161588.90639746, 601815.56244548 6161594.43527207, 601800.7116799 6161617.70053649, 601784.22823715 6161642.30552822, 601778.61281719 6161650.08394495, 601777.75524938 6161654.50304545, 601775.68554246 6161661.23167586, 601771.15857394 6161674.99887358, 601771.39770343 6161680.1278296, 601771.80999564 6161690.01581693, 601771.23278654 6161695.68466305, 601768.89096676 6161703.85300041, 601767.43145232 6161718.54001091, 601767.15109362 6161723.80893844, 601767.92620298 6161729.78772147, 601769.65783028 6161735.55654724, 601771.1503281 6161741.09541982, 601770.29276029 6161749.5437002, 601768.71780403 6161772.47903178, 601770.15258094 6161778.53779853, 601770.53188978 6161792.88487823, 601766.94494751 6161810.90121106, 601760.85951442 6161831.01711652, 601756.47272525 6161837.79573675, 601749.43902007 6161847.90367931, 601742.27338137 6161855.44214488, 601735.28090541 6161861.75086076, 601727.40612411 6161868.84941587, 601722.59055104 6161873.45847771, 601719.88591411 6161878.06753954, 601713.05010919 6161894.54418577, 601700.64011352 6161931.93657466, 601692.20461481 6161950.79273654, 601686.70463666 6161970.78866643, 601686.31708198 6161977.98720119, 601687.85905486 6161984.1159537, 601691.28932609 6161990.0747408, 601694.95048096 6161998.12310258, 601697.73757633 6162004.24185713, 601697.47370931 6162012.54016803, 601695.23083967 6162024.97763642, 601691.13265505 6162035.1355688, 601684.52773377 6162046.84318575, 601678.14545028 6162062.71995408, 601673.70094021 6162070.8982894, 601671.73018342 6162079.60651687, 601671.00454912 6162088.65467514, 601670.98805743 6162099.82240198, 601672.3403759 6162109.2004931, 601674.04726567 6162119.27844176, 601678.07123768 6162138.81446526, 601679.67093148 6162145.92301834, 601680.92429981 6162155.49107079, 601679.49776875 6162181.80571452, 601676.00977661 6162195.4129448, 601671.63947913 6162205.67085684, 601660.07055959 6162216.33868543, 601655.09831548 6162221.77757836, 601650.82696813 6162228.0962922, 601648.52637757 6162235.79472521, 601648.69129446 6162242.07344719, 601650.4311676 6162253.8510499, 601654.29022274 6162268.61804412, 601655.90640822 6162273.62702455, 601657.51434785 6162286.03449905, 601658.8996497 6162290.44360159, 601660.45811427 6162295.38259627, 601665.96633826 6162298.55195116, 601670.93858237 6162302.591129, 601672.16721317 6162306.2403862, 601673.21443539 6162314.29874594, 601675.66345115 6162320.89740281, 601676.40557714 6162323.17693881, 601678.20317119 6162325.46647279, 601681.45203385 6162325.79640563, 601682.54873114 6162322.60705481, 601681.88906359 6162316.24834911, 601680.56148266 6162310.21957625, 601681.91380113 6162304.93065279, 601684.32983351 6162301.05144239, 601686.68814497 6162295.75252097, 601687.94151331 6162287.92411442, 601689.27734008 6162278.06612099, 601689.90402425 6162268.38809092, 601689.13716073 6162259.77984311, 601687.6611546 6162253.28116589, 601681.68291749 6162240.55375652, 601679.72040654 6162232.16546393, 601675.33361738 6162229.18607038, 601673.4205815 6162226.92653031, 601674.24516593 6162225.28686405, 601677.15594897 6162222.93734229, 601682.36732256 6162220.04793042, 601688.55170579 6162214.98896016, 601696.12139085 6162212.76941195, 601705.10111529 6162205.84082224, 601709.63632965 6162205.23094638, 601709.24877497 6162215.1789215, 601706.1648292 6162241.42357947, 601707.50065598 6162268.54805836, 601705.22480295 6162291.753335, 601705.34024477 6162317.70805199, 601702.64385369 6162344.01269775, 601700.40922988 6162364.39854827, 601698.27355621 6162373.93660683, 601696.31929111 6162379.95538173, 601693.46622899 6162385.27429908, 601685.34407235 6162395.99211749, 601679.81935668 6162406.84990742, 601676.76839429 6162412.94866604, 601669.5862639 6162443.66241434, 601666.16423852 6162471.21680573, 601665.21596643 6162479.15518989, 601666.03230501 6162487.45350079, 601669.47082208 6162503.91015109, 601673.25566462 6162522.48636995, 601673.53602332 6162538.80304874, 601671.82088771 6162548.35110526, 601667.16198568 6162557.4392554, 601661.79394104 6162564.40783696, 601651.18153944 6162571.87631677, 601647.81723496 6162573.96589145, 601646.56386663 6162576.76532163, 601643.3562332 6162579.53475792, 601637.65010895 6162583.66391744, 601634.78880097 6162586.60331913, 601629.27233114 6162586.32337612, 601608.10524883 6162581.29439975, 601592.24024441 6162586.85326826, 601584.90968883 6162583.27399681, 601563.5848645 6162572.04337913, 601554.14444376 6162554.31989033, 601552.03350762 6162549.29091397, 601552.70142101 6162535.30376101, 601548.47130288 6162490.86280684, 601554.68042364 6162465.70792704, 601564.99597485 6162441.37288037, 601573.34076928 6162421.55691385, 601575.97943946 6162409.39938848, 601580.39990341 6162398.13003071, 601585.11150581 6162377.94786601, 601573.83551994 6162357.30999113, 601566.46518871 6162319.98979449, 601576.51541933 6162300.41157264, 601586.39712239 6162273.24676429, 601587.44940887 6162260.7796396, 601594.19099652 6162232.19057404, 601580.21780342 6162208.89020155, 601586.39712239 6162196.34124998, 601589.05330448 6162188.54623799, 601593.97187171 6162164.45924534, 601599.78519194 6162161.27989249, 601593.44413767 6162154.93118475, 601588.15030564 6162104.68141295, 601592.38042376 6162104.1515208, 601590.26948762 6162086.69507401, 601591.32495569 6162073.46776639, 601605.29548864 6162054.47366533, 601594.51100691 6162046.41077518, 601592.9081578 6162033.78584353, 601614.60297414 6162008.93090268, 601613.01152619 6162003.63198126, 601587.87285152 6162032.28105808, 601578.6168592 6162078.92354417, 601578.98879308 6162042.70661039, 601576.51541933 6162033.26594936, 601572.27705537 6162025.32756519, 601573.34076928 6162013.689934, 601565.25859391 6161986.64241244, 601558.45779225 6161953.03883042, 601552.70966685 6161926.92759422, 601547.28514364 6161911.10468535, 601543.46103176 6161896.05989213, 601536.12509802 6161871.66998182, 601530.54483738 6161858.82145703, 601520.05020805 6161843.89099071, 601516.51865624 6161839.18545388, 601495.58023148 6161823.48597077, 601493.61661697 6161823.63172791, 601482.41205051 6161795.23133426, 601477.56528778 6161780.94730809, 601470.51114867 6161762.34316415, 601463.45700956 6161738.800134, 601460.84271555 6161723.18906461), (601486.8621897 6161724.37784141, 601487.33858578 6161725.1321352, 601491.15978518 6161731.18236758, 601496.38718455 6161739.45908324, 601499.60396563 6161748.72560602, 601511.90939839 6161784.1736424, 601525.54516472 6161819.33956614, 601526.63643503 6161822.15389484, 601528.66633379 6161827.38889693, 601531.3121657 6161839.73611248, 601530.34219111 6161844.10099811, 601529.54827776 6161847.67360815, 601530.00751368 6161849.05131592, 601532.19410966 6161855.61110386, 601533.15041915 6161860.87080607, 601533.95799759 6161865.31248748, 601536.21487006 6161869.37485793, 601538.36771744 6161873.2499832, 601543.94293165 6161883.60395244, 601553.87533257 6161902.04983988, 601556.88854077 6161907.64579797, 601558.97584693 6161916.33168491, 601561.02375498 6161924.85362484, 601573.84097725 6161978.18980783, 601584.22880379 6162021.41656988, 601584.26296447 6162021.3935598, 601597.67735312 6162012.3578465, 601642.66792696 6161982.05293011, 601643.83603602 6161987.36728657, 601646.13472468 6161997.82525774, 601648.04451232 6162006.94614312, 601648.10224763 6162014.88307775, 601648.10927418 6162015.84902458, 601648.11746218 6162016.97463841, 601645.87949695 6162022.71589391, 601645.72096893 6162023.12258024, 601645.46380443 6162025.38376628, 601643.36039078 6162043.87858032, 601635.50264898 6162076.48122384, 601631.87823824 6162093.56165353, 601628.54665967 6162097.4994274, 601623.5375066 6162103.4200178, 601619.69053052 6162107.96696802, 601617.18732549 6162129.01649007, 601616.91251566 6162130.02357362, 601611.96940283 6162148.13838274, 601611.47536025 6162149.94887894, 601609.20297651 6162159.85696821, 601602.69003025 6162174.91457472, 601600.9127032 6162179.96529068, 601601.03423634 6162179.90348631, 601600.78286641 6162180.63368021, 601603.06101682 6162179.38952081, 601615.48902191 6162172.60225196, 601616.80836519 6162171.881723, 601620.34438709 6162179.95689357, 601621.39973668 6162181.55547941, 601623.19970288 6162185.34790496, 601623.27134545 6162197.84635955, 601623.1583119 6162203.63544663, 601623.54767962 6162262.69847081, 601629.4663713 6162275.43686545, 601629.7860039 6162276.22015965, 601634.05243682 6162285.01170747, 601643.22982173 6162308.76562802, 601649.76137474 6162324.58205285, 601650.9032642 6162333.11872234, 601651.25338279 6162335.73617943, 601648.03338206 6162331.311162, 601639.46419469 6162322.10715656, 601641.92055866 6162342.03568662, 601644.75802673 6162362.83886574, 601641.05564264 6162388.76358884, 601636.28954464 6162398.81154361, 601625.18239237 6162417.32777468, 601627.11154054 6162438.99824505, 601629.41251049 6162459.11926815, 601629.41251049 6162463.88829742, 601624.1269243 6162461.76872886, 601622.00774232 6162446.95174481, 601617.24989016 6162435.84400576, 601618.30535823 6162425.79605099, 601613.54750607 6162411.50895909, 601607.72594 6162424.20637456, 601607.19820596 6162436.90379004, 601603.49582187 6162449.07131338, 601598.72972387 6162460.70894457, 601597.6742558 6162469.70711302, 601610.90059005 6162485.57388338, 601623.06321039 6162475.52592862, 601632.05942651 6162481.87463636, 601628.88477646 6162494.57205183, 601618.3548131 6162531.34888098, 601616.18617624 6162539.53290018, 601616.71391028 6162547.47128435, 601612.48379215 6162564.397839, 601606.66222608 6162571.80633102, 601588.67803967 6162575.50557805, 601599.90063376 6162578.60494718, 601612.48379215 6162578.15503876, 601623.59094442 6162562.28826839, 601627.26783624 6162564.10091584, 601631.31673703 6162564.02138593, 601642.97024604 6162564.0415885, 601660.23210801 6162552.45339907, 601660.24065143 6162552.4128766, 601660.99047642 6162550.43788305, 601663.5949464 6162539.74558599, 601663.7650311 6162535.69631617, 601664.08842167 6162534.16243548, 601663.88759364 6162531.06377582, 601663.3849573 6162523.30838978, 601663.00584911 6162517.45897104, 601661.60347872 6162511.02184086, 601657.76100592 6162493.38420546, 601657.09176877 6162490.31228755, 601656.98439033 6162489.81940138, 601665.380934 6162425.9138405, 601682.17402135 6162369.36379136, 601675.7604628 6162337.8270598, 601674.63856806 6162327.45698348, 601670.39256356 6162313.49188877, 601669.84428965 6162311.6886177, 601669.81972182 6162311.65648803, 601668.31717439 6162309.69146478, 601664.62492974 6162304.86276757, 601663.3182318 6162304.63665278, 601661.87501294 6162304.38691401, 601659.59137953 6162305.38831259, 601658.37186949 6162305.92308126, 601654.95050589 6162303.63342406, 601654.86872604 6162303.57869505, 601653.22009589 6162298.03805733, 601647.60515325 6162279.15769739, 601641.53934207 6162258.76371704, 601641.51244256 6162258.67327768, 601642.6169446 6162250.16578354, 601642.91864256 6162247.84193661, 601644.99502236 6162231.84849493, 601645.84430329 6162225.3068569, 601660.75949163 6162206.0515396, 601664.63120787 6162197.30819261, 601670.45724662 6162184.15147471, 601671.60189029 6162181.56657019, 601671.59731481 6162181.51482479, 601667.73409262 6162137.82451443, 601667.62763028 6162136.62050072, 601663.76313076 6162115.69805108, 601658.97355509 6162089.76722741, 601659.09473624 6162089.43976078, 601660.13830595 6162086.61973271, 601680.67648084 6162031.11962653, 601687.85722005 6162011.71518624, 601687.80696465 6162011.51121926, 601676.32371611 6161964.90521422, 601675.81868488 6161962.85549041, 601675.75877771 6161962.85715066, 601656.72459797 6161963.38465679, 601656.74646999 6161963.12550655, 601658.16606614 6161946.30544291, 601658.17980551 6161946.1426522, 601680.25045332 6161929.05498781, 601680.43798328 6161926.35634234, 601682.54938613 6161895.97224983, 601682.56555623 6161895.73955441, 601684.99215963 6161891.72724978, 601692.97613691 6161878.52602001, 601704.83464143 6161858.91839373, 601714.30015822 6161841.08372989, 601717.75848628 6161847.57374647, 601717.75848628 6161854.45234635, 601703.49899948 6161875.59934959, 601695.01644771 6161890.95491636, 601693.42499976 6161901.53276327, 601686.02023159 6161927.98737851, 601677.99968277 6161943.02069571, 601675.96854739 6161946.50360958, 601668.56377921 6161947.03350172, 601659.56756308 6161951.79253303, 601658.51209501 6161957.08145649, 601679.67093148 6161953.9121016, 601693.9527338 6161934.86597839, 601698.7188318 6161920.57888648, 601699.41397424 6161917.27484884, 601702.94894993 6161900.47297898, 601708.770516 6161886.72577719, 601714.05610219 6161876.66782445, 601721.99685025 6161869.78922457, 601729.92935246 6161865.5600854, 601732.57626848 6161855.51213063, 601735.59132525 6161850.64223132, 601739.45330262 6161844.40439158, 601750.03272086 6161831.17708396, 601756.19746808 6161814.87931697, 601762.33051914 6161800.29018297, 601765.90597113 6161792.02505324, 601764.31452318 6161769.279683, 601760.08440505 6161759.75162241, 601756.38202096 6161733.30700513, 601761.13987312 6161725.89851311, 601763.14747368 6161693.79910501, 601764.13234464 6161681.07628394, 601765.90597113 6161668.24024928, 601759.55667102 6161660.30186512, 601735.75091854 6161640.19595762, 601737.86185468 6161645.48488107, 601755.31830705 6161669.2900356, 601759.02069114 6161681.98745108, 601754.79057302 6161697.33432727, 601738.38958871 6161720.07969751, 601734.15947059 6161733.83689727, 601727.28243644 6161746.53431275, 601727.28243644 6161757.6420518, 601726.91584949 6161762.05213964, 601726.9631019 6161776.4345648, 601726.35407426 6161775.35420273, 601720.50633589 6161744.10935252, 601720.31275807 6161743.07505349, 601722.70785517 6161740.51214704, 601727.23601823 6161735.66672416, 601748.27038187 6161693.68619128, 601721.45928523 6161691.56952574, 601718.60331131 6161689.73306024, 601702.80617031 6161679.57508778, 601705.33622594 6161676.23018794, 601723.88664232 6161651.7053182, 601726.28792846 6161648.5306601, 601732.63437463 6161638.9969376, 601738.73436271 6161629.83344798, 601738.89769254 6161629.89782197, 601744.33191787 6161632.03963981, 601748.13257812 6161633.53761264, 601746.69614271 6161612.1037043, 601746.46180015 6161608.6069394, 601746.12891167 6161603.63971212, 601746.77787709 6161588.80621686, 601746.88926076 6161586.26030442, 601746.89096638 6161586.22131875, 601771.3831167 6161580.13487601, 601782.33649362 6161585.15351416, 601783.415849 6161585.64805517, 601795.53553705 6161584.12018989, 601809.30090447 6161582.38486248, 601803.83285187 6161528.93905805, 601806.21410058 6161516.41545372, 601816.04178701 6161495.25639115, 601822.3536752 6161481.6668614, 601822.47650292 6161481.58821282, 601827.24435075 6161478.53528258, 601866.98003999 6161453.09187678, 601878.05654161 6161448.41598337, 601887.70849331 6161444.34145706, 601892.46822067 6161442.33216039, 601908.64763896 6161437.12616461, 601920.42584447 6161433.33633193, 601966.90429159 6161431.48424957, 602003.12822813 6161435.74987198, 602014.08829388 6161437.04049654, 602014.34636958 6161437.23383311, 602014.49960975 6161437.34863249, 602044.48699308 6161459.81311707, 602065.77021042 6161475.75783672, 602082.62846252 6161504.42481729, 602084.46742255 6161507.55191678, 602085.24919347 6161508.45312492, 602091.28593475 6161515.41214612, 602093.9924174 6161518.53211918, 602170.99248237 6161516.92876588, 602180.63410899 6161516.72800067, 602180.81980107 6161516.72413405, 602199.64930479 6161515.70989852, 602201.60953285 6161516.07108537, 602209.21905217 6161520.11040069, 602222.9813663 6161522.22996925, 602233.44322823 6161520.21468973, 602236.73543459 6161519.58050854, 602257.89427105 6161507.41298521, 602256.3028231 6161489.42664627, 602259.47747316 6161484.66761496, 602258.42200509 6161471.97019948, 602254.719621 6161457.15321544, 602249.08982011 6161449.42290823, 602228.79468653 6161406.37355149, 602202.95848992 6161373.85814721, 602196.00096376 6161365.10195221, 602181.82955119 6161354.32206416, 602171.6592314 6161346.58572113, 602168.21823935 6161349.54060847, 602135.33496553 6161344.33574422, 602127.68267698 6161342.49610313, 602116.96865924 6161340.1511483, 602082.91408066 6161332.24018727, 602034.04284341 6161329.26811247, 602016.58554971 6161335.22817411, 602011.10869707 6161332.35523872, 602007.16431033 6161329.45741968, 602010.42466915 6161323.77982468, 602016.92153888 6161313.45809797, 602014.91127586 6161312.78825247, 602014.72731514 6161312.28484453, 601982.83364989 6161301.32235878, 601966.47308366 6161292.32255732, 601951.26751552 6161283.51823934, 601953.01637104 6161268.63111073, 601953.55386476 6161265.92131811, 601945.58212284 6161264.17542244, 601937.53285433 6161262.36553087, 601942.00247302 6161249.02007584, 601952.6496069 6161216.45220943, 601956.87972502 6161194.22673336, 601963.07635015 6161182.05414628, 601966.93140922 6161165.66254752, 601968.77233281 6161151.16047099, 601971.16152734 6161132.33933036, 601970.63379331 6161116.99245418, 601979.63000944 6161073.08139215, 601981.74094557 6161057.21462178, 601991.79262977 6161026.53086737, 601994.43954579 6161013.30355975, 601990.21894027 6161000.63767401, 601986.50704358 6160989.49840522, 601981.21321154 6160989.49840522, 601973.6526279 6160995.72463665, 601965.36470736 6161004.95940635, 601949.61921023 6161023.96469734, 601939.95100668 6161038.69839071, 601920.37537232 6161049.80612976, 601906.09357 6161048.74634548, 601885.46246758 6161051.92569833, 601870.11695134 6161061.97365309, 601856.89061709 6161082.07956059, 601842.14077009 6161093.99131617, 601825.76180572 6161106.00484748, 601803.99352593 6161122.28137763, 601794.46957577 6161135.50868525, 601792.35039378 6161144.5068537, 601787.59254163 6161161.43340835, 601787.59254163 6161171.48136312, 601793.97023693 6161184.67224269, 601801.34660991 6161201.10533324, 601801.34660991 6161212.21307229, 601799.76340781 6161221.21124074, 601795.52504384 6161232.84887194, 601800.29114184 6161253.48467158, 601805.57672803 6161267.23187338, 601808.759010492 6161269.96459872, 601811.36133619 6161278.97751473, 601813.87189652 6161293.07188854, 601815.38631785 6161301.57390304, 601818.91710683 6161315.90943726, 601821.25124524 6161325.3863902, 601825.43851564 6161327.94384044, 601840.73488058 6161337.28636872, 601843.6285053 6161339.05370174, 601846.23285407 6161340.68920798, 601846.51893992 6161364.36831361, 601839.13804108 6161375.84760357, 601830.05628511 6161389.97218548, 601829.38531533 6161391.0157246, 601829.36512974 6161391.04711864, 601821.86769601 6161393.21142046, 601793.16132985 6161401.49815467, 601789.62560327 6161404.92774312, 601766.49452684 6161427.36445328, 601746.15371634 6161447.09465783, 601737.11284561 6161451.95179758, 601720.09541477 6161461.0942842, 601718.32838415 6161462.0436081, 601709.02901421 6161468.56704672, 601702.81719461 6161472.92459181, 601698.58129347 6161478.62682003, 601687.46654423 6161493.58912314, 601670.53475093 6161516.38213609, 601669.31434812 6161518.02500148, 601668.40569763 6161521.3634135, 601662.23246503 6161544.04407471, 601658.80391387 6161557.94459805, 601657.34195875 6161562.01195689, 601655.842654 6161564.21681675, 601653.62719435 6161566.56871233, 601641.76322721 6161579.16330503, 601624.71340289 6161597.26311855, 601595.69407537 6161628.06956012, 601569.89001434 6161650.90427214, 601524.65157931 6161690.93698773, 601486.8621897 6161724.37784141), (601591.94230482 6162243.75106057, 601594.07050545 6162256.32417271, 601597.14652176 6162274.4968475, 601602.95984199 6162288.25404727, 601611.42832408 6162290.36361787, 601628.35704242 6162299.89167846, 601627.30157435 6162296.18243347, 601628.35704242 6162283.48501799, 601620.42454021 6162276.08652393, 601613.54750607 6162261.79943203, 601616.4831357 6162250.2808507, 601618.30535823 6162234.81492465, 601618.00859425 6162231.26331415, 601617.14782234 6162211.7625712, 601618.83309226 6162196.20278403, 601613.54750607 6162178.74633725, 601594.24366998 6162232.04514013, 601591.94230482 6162243.75106057), (601612.48379215 6162098.86259735, 601610.90059005 6162107.32087569, 601618.51284828 6162100.94480497, 601630.47622441 6162090.92421318, 601630.12394198 6162084.32001389, 601630.47622441 6162066.58916651, 601635.7618106 6162057.07110389, 601641.58337667 6162026.91724162, 601641.48648158 6162021.2835226, 601641.05564264 6161996.23348721, 601638.40872662 6161986.17553447, 601630.47622441 6162007.8711184, 601622.00774232 6162021.09842602, 601625.18239237 6162045.95336687, 601622.00774232 6162068.17884294, 601616.71391028 6162081.92604473, 601612.48379215 6162098.86259735), (601770.66382329 6161602.10371118, 601771.19155732 6161613.74134238, 601779.12405953 6161622.73951083, 601779.66003941 6161637.02660273, 601784.41789157 6161623.79929511, 601803.99352593 6161606.87274046, 601808.22364405 6161590.99597213, 601802.92981202 6161590.99597213, 601795.52504384 6161597.87457201, 601787.59254163 6161596.28489559, 601777.01312339 6161592.58564856, 601770.66382329 6161602.10371118), (601972.21699541 6160895.33757139, 601972.55861672 6160900.43332759, 601973.28070933 6160911.20434175, 601985.44332966 6160922.84197295, 601997.7357892 6160928.4586077, 602007.08439525 6160926.1655535, 602022.60660909 6160884.5377982, 602026.71378037 6160852.48629364, 602034.1829217 6160824.61723015, 602040.46784865 6160801.16673959, 602037.2931986 6160795.34792399, 602019.30076634 6160799.04717102, 602003.43576192 6160801.16673959, 601981.74094557 6160824.97189412, 601976.45535938 6160844.54790948, 601983.33239352 6160842.42834091, 601988.61797972 6160828.15124697, 601997.08646181 6160816.51361578, 602011.15224915 6160809.9487175, 602032.52710059 6160803.28630816, 602032.52710059 6160812.80437078, 602023.00315043 6160820.2128628, 602021.94768236 6160828.15124697, 602012.93995327 6160850.43374496, 602009.11577248 6160862.25291582, 601995.49501386 6160907.50509472, 601989.14571375 6160911.20434175, 601982.26867961 6160906.97520258, 601979.21496587 6160899.70723431, 601989.14759219 6160886.13838416, 601985.97930954 6160856.71543281, 601974.3361774 6160856.71543281, 601975.12664236 6160869.85598942, 601976.98309342 6160884.21983437, 601972.21699541 6160895.33757139))");
            result.Add(polygon);

            return result;
        }

    }
}