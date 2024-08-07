$(document).ready(function () {
    ComboDoldur("Nota", $("#cmbNotalar"), false);
    ComboDoldur("Mod", $("#cmbModlar"), true);
    ComboDoldur("Akor", $("#cmbAkorlar"), true);
    ComboDoldur("Aralik", $("#cmbAraliklar"), true);
    ComboDoldur("Plan", $("#cmbPlanlar"), true);

    $("#btnGiris").click(function () {
        var sifre = $("#txtSifre").val();

        GirisKontrol(sifre);
    });

    $("#txtSifre").keyup(function (event) {
        if (event.keyCode == 13) {
            var sifre = $("#txtSifre").val();

            GirisKontrol(sifre);
        }
    });

    $("#btnGoster").click(function () {
        $(".fretboard").css("visibility", "visible");
    });

    ModAkorGetir("mod", 1, 2, "C", "Ionian (Major)");
});

$(function () {
    $("#btnModEkle").click(function () {
        var nota = $("#cmbNotalar").val();
        var mod = $("#cmbModlar").val();
        var notaIsim = $("#cmbNotalar option:selected").text();
        var modIsim = $("#cmbModlar option:selected").text();

        ModAkorGetir("mod", nota, mod, notaIsim, modIsim);
    });

    $("#btnAkorEkle").click(function () {
        var nota = $("#cmbNotalar").val();
        var akor = $("#cmbAkorlar").val();
        var notaIsim = $("#cmbNotalar option:selected").text();
        var akorIsim = $("#cmbAkorlar option:selected").text();

        ModAkorGetir("akor", nota, akor, notaIsim, akorIsim);
    });

    $("#btnAralikEkle").click(function () {
        var nota = $("#cmbNotalar").val();
        var aralik = $("#cmbAraliklar").val();
        var notaIsim = $("#cmbNotalar option:selected").text();
        var aralikIsim = $("#cmbAraliklar option:selected").text();

        ModAkorGetir("aralık", nota, aralik, notaIsim, aralikIsim);
    });

    $("#btnTemizle").click(function () {
        FretboardTemizle();
    });

    $("#btnPlanEkle").click(function () {
        var planlar = [];
        var isim = $("#txtPlan").val();

        if (isim == "" || $(".fretboard").length <= 0) {
            alert("Plan ismi boş veya plana henüz bir mod veya akor bilgisi eklemediniz.\nLütfen önce bu bilgileri eksiksiz tamamlayınız.");
            return false;
        }

        $(".fretboard").each(function (i) {
            var plan = new Object();
            plan.Tip = $(this).attr("data-tip");
            plan.TipID = parseInt($(this).attr("data-tip-id"));
            plan.NotaID = parseInt($(this).attr("data-nota"));

            planlar[i] = plan;
        });

        $.ajax({
            type: "POST",
            url: MainPath + "/Ajax/PlanKaydet",
            data: "{isim: '" + isim + "', plan: '" + JSON.stringify(planlar) + "' }",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (planID) {
                if (planID > 0) {
                    alert("Planınız başarıyla kaydedildi.");

                    $("#cmbPlanlar").append("<option value=\"" + planID + "\">" + isim + "</option>");
                    $("#txtPlan").val("");
                }
                else {
                    alert("Planınız kaydedilirken hata meydana geldi.");
                }
            }
        });
    });

    $("#btnPlanSil").click(function () {
        if ($("#cmbPlanlar").val() != "-") {
            var planID = parseInt($("#cmbPlanlar").val());

            $.ajax({
                type: "POST",
                url: MainPath + "/Ajax/PlanSil",
                data: "{planID: " + planID + "}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    if (result == true) {
                        alert("Plan başarıyla silindi.");
                        $("#cmbPlanlar option[value=\"" + planID + "\"]").remove();
                    }
                    else {
                        alert("Plan silinirken hata meydana geldi.");
                    }
                }
            });
        }
        else {
            alert("Plan Geçersiz.");
        }
    });

    $("#btnPlanGetir").click(function () {
        if ($("#cmbPlanlar").val() != "-") {
            FretboardTemizle();

            var zaman = 0;

            if ($(".fretboard").length > 0) {
                zaman = 1000;
            }

            setTimeout(function () {
                var planID = parseInt($("#cmbPlanlar").val());

                if (planID == NaN) {
                    alert("Plan Geçersiz.");
                }

                $.ajax({
                    type: "POST",
                    url: MainPath + "/Ajax/PlanGetir",
                    data: "{planID: " + planID + "}",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (result) {
                        if (result != null) {
                            for (var i = 0; i < result.length; i++) {
                                ModAkorGetir(result[i].Tip, result[i].NotaID, result[i].TipID, result[i].NotaIsim, result[i].TipIsim)
                            }
                        }
                    }
                });
            }, zaman);
        }
        else {
            alert("Plan Geçersiz.");
        }
    });

    $(".btnFullTranspoze").click(function () {
        var yon = $(this).attr("data-yon");

        $(".btnTranspoze[data-yon=\"" + yon + "\"]").each(function () {
            $(this).click();
        });
    });

    $(document).on("click", "#btnSirala", function () {
        AkorSirala();
    });

    $(document).on("click", ".btnEkle.Mod", function () {
        var nota = $("#cmbNotalar").val();
        var mod = $("#cmbModlar").val();
        var notaIsim = $("#cmbNotalar option:selected").text();
        var modIsim = $("#cmbModlar option:selected").text();
        var fbID = $(this).parent(".kontrol").next(".fretboard").attr("id");

        ModAkorGetir("mod", nota, mod, notaIsim, modIsim, fbID);
    });

    $(document).on("click", ".btnEkle.Akor", function () {
        var nota = $("#cmbNotalar").val();
        var akor = $("#cmbAkorlar").val();
        var notaIsim = $("#cmbNotalar option:selected").text();
        var akorIsim = $("#cmbAkorlar option:selected").text();
        var fbID = $(this).parent(".kontrol").next(".fretboard").attr("id");

        ModAkorGetir("akor", nota, akor, notaIsim, akorIsim, fbID);
    });

    $(document).on("click", ".btnEkle.Aralik", function () {
        var nota = $("#cmbNotalar").val();
        var aralik = $("#cmbAraliklar").val();
        var notaIsim = $("#cmbNotalar option:selected").text();
        var aralikIsim = $("#cmbAraliklar option:selected").text();
        var fbID = $(this).parent(".kontrol").next(".fretboard").attr("id");

        ModAkorGetir("aralık", nota, aralik, notaIsim, aralikIsim, fbID);
    });

    $(document).on("click", ".btnAkor", function () {
        $("#loader").addClass("acik");

        var tip = parseInt($(this).attr("data-tip"));
        var nota = parseInt($(this).parent(".kontrol").next(".fretboard").attr("data-nota"));
        var modid = parseInt($(this).parent(".kontrol").next(".fretboard").attr("data-tip-id"));

        BagliAkorlar(tip, nota, modid);
    });

    $(document).on("click", ".btnMod", function () {
        $("#loader").addClass("acik");

        var nota = parseInt($(this).parent(".kontrol").next(".fretboard").attr("data-nota"));
        var modid = parseInt($(this).parent(".kontrol").next(".fretboard").attr("data-tip-id"));

        BagliModlar(nota, modid);
    });

    $(document).on("click", ".btnDegistir.Mod", function () {
        var nota = $("#cmbNotalar").val();
        var mod = $("#cmbModlar").val();
        var notaIsim = $("#cmbNotalar option:selected").attr("data-diyez");
        var modIsim = $("#cmbModlar option:selected").text();
        var fbID = $(this).parent(".kontrol").next(".fretboard").attr("id");

        ModAkorGetir("mod", nota, mod, notaIsim, modIsim, fbID, true);
    });

    $(document).on("click", ".btnDegistir.Akor", function () {
        var nota = $("#cmbNotalar").val();
        var akor = $("#cmbAkorlar").val();
        var notaIsim = $("#cmbNotalar option:selected").text();
        var akorIsim = $("#cmbAkorlar option:selected").text();
        var fbID = $(this).parent(".kontrol").next(".fretboard").attr("id");

        ModAkorGetir("akor", nota, akor, notaIsim, akorIsim, fbID, true);
    });

    $(document).on("click", ".btnDegistir.Aralik", function () {
        var nota = $("#cmbNotalar").val();
        var aralik = $("#cmbAraliklar").val();
        var notaIsim = $("#cmbNotalar option:selected").text();
        var aralikIsim = $("#cmbAraliklar option:selected").text();
        var fbID = $(this).parent(".kontrol").next(".fretboard").attr("id");

        ModAkorGetir("aralık", nota, aralik, notaIsim, aralikIsim, fbID, true);
    });

    $(document).on("click", ".btnYerlestir", function () {
        var yon = $(this).attr("data-yon");

        var fret = $(this).parent(".kontrol").next(".fretboard").parent(".fretframe");
        var fretIleri = $(this).parent(".kontrol").next(".fretboard").parent(".fretframe").next(".fretframe");
        var fretGeri = $(this).parent(".kontrol").next(".fretboard").parent(".fretframe").prev(".fretframe");

        if (yon == ">") {
            if (fretIleri.length > 0) {
                fret.insertAfter(fretIleri);
            }
        }
        else if (yon == "<") {
            if (fretGeri.length > 0) {
                fret.insertBefore(fretGeri);
            }
        }
    });

    $(document).on("click", ".btnTranspoze", function () {
        var fretboard = $(this).parent(".kontrol").next(".fretboard");
        var yon = $(this).attr("data-yon");
        var eksennota = 0;
        var notalar = fretboard.attr("data-notalar").split(",");
        var yeninotalar = "";

        fretboard.find(".perde").removeClass("aktif");
        fretboard.find(".perde").removeClass("eksen");

        if (yon == "-") {
            eksennota = parseInt(fretboard.attr("data-nota")) - 1 == 0 ? 12 : parseInt(fretboard.attr("data-nota")) - 1;
            fretboard.find(".perde[data-notaid=\"" + eksennota.toString() + "\"]").addClass("eksen");
        }
        else {
            eksennota = parseInt(fretboard.attr("data-nota")) + 1 == 13 ? 1 : parseInt(fretboard.attr("data-nota")) + 1;
            fretboard.find(".perde[data-notaid=\"" + eksennota.toString() + "\"]").addClass("eksen");
        }

        for (var i = 0; i < notalar.length; i++) {
            if (yon == "-") {
                var prevNote = parseInt(notalar[i]) - 1 == 0 ? 12 : parseInt(notalar[i]) - 1;

                yeninotalar += prevNote + ",";

                fretboard.find(".perde[data-notaid=\"" + prevNote.toString() + "\"]").addClass("aktif");
            }
            else {
                var nextNote = parseInt(notalar[i]) + 1 == 13 ? 1 : parseInt(notalar[i]) + 1;

                yeninotalar += nextNote + ",";

                fretboard.find(".perde[data-notaid=\"" + nextNote.toString() + "\"]").addClass("aktif");
            }
        }

        yeninotalar = yeninotalar.slice(0, -1);

        fretboard.attr("data-notalar", yeninotalar);
        fretboard.attr("data-nota", eksennota);
        fretboard.prev(".kontrol").find(".notabaslik").text(fretboard.find(".perde[data-notaid=\"" + eksennota.toString() + "\"]").first().attr("data-nota"));
        fretboard.find(".perde[data-notaid=\"" + eksennota.toString() + "\"]").addClass("eksen");

        var yeninotalarid = yeninotalar.split(',');
        var tempnotalar = "";

        for (var i = 0; i < yeninotalarid.length; i++) {
            tempnotalar += fretboard.find(".perde[data-notaid=\"" + yeninotalarid[i].toString() + "\"]").first().attr("data-nota") + " - ";
        }

        tempnotalar = tempnotalar.slice(0, -3);

        fretboard.prev(".kontrol").find(".notalar").text("(" + tempnotalar + ")");

        var isNum = $(".numerik input[type='checkbox']").is(":checked");
        var isBemol = $(".bemol input[type='checkbox']").is(":checked");

        if (isNum == true) {
            NumerikYap(true, fretboard);
        }

        if (isBemol == true) {
            BemolYap(true, fretboard);
        }
    });

    $(document).on("click", ".btnKaldir", function () {
        $(this).parent().parent().fadeOut("slow", function () {
            $(this).remove();
        });
    });

    $(document).on("click", ".btnDigerKaldir", function () {
        var id = $(this).parent().next(".fretboard").attr("id");

        $.each($(".fretboard"), function () {
            if (id != $(this).attr("id")) {
                $(this).parent().fadeOut("slow", function () {
                    $(this).remove();
                });
            }
        });
    });

    $(document).on("click", ".btnGizle", function () {
        var fretDiv = $(this).parent().next(".fretboard");

        if (fretDiv.css("visibility") == "visible") {
            fretDiv.css("visibility", "hidden");
        }
        else {
            fretDiv.css("visibility", "visible");
        }
    });

    $(document).on("click", ".btnCal", function (e) {
        var notalar = $(this).parent().next(".fretboard").attr("data-notalar");

        notalar = Sirala(notalar);
        notalar += "," + (parseInt(notalar[0]) + 12).toString();

        notalar = notalar.split(',');

        var i = 0;
        var times = notalar.length;
        var loop = setInterval(cal, 500);

        function cal() {
            var calanses = notalar[i];

            times--;
            if (times === 0) {
                clearInterval(loop);
            }

            var audio = document.getElementById("ses" + calanses);
            audio.play();
            i++;
        }
    });

    $(document).on("click", ".btnAra", function (e) {
        var tel = $(this).parent().next(".fretboard").children(".tel").first();

        var formul = FormulDon(tel);

        $.ajax({
            type: "POST",
            url: MainPath + "/Ajax/ModAkorKontrol",
            data: "{formul: '" + formul + "'}",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (answer) {
                if (answer == true) {
                    alert("Bu formülde mod veya akor mevcut.");
                }
                else {
                    alert("Bu formülde mod veya akor mevcut değil.");
                }
            }
        });
    });

    $(document).on("click", ".btnOdaklan", function () {
        var frets = $(".fretboard");
        var fretCount = frets.length;
        var thisFret = $(this).parent().next(".fretboard");
        var thisFretID = $(this).parent().next(".fretboard").attr("id");
        var kontrol = true;

        frets.each(function () {
            if ($(this).attr("id") != thisFretID) {
                if ($(this).css("opacity") == "0.3" && thisFret.css("opacity") == "1") {
                    $(".fretboard").css("opacity", "1");
                    kontrol = false;
                }
            }
        });

        if (kontrol == false) {
            return false;
        }

        frets.css("opacity", "1")

        frets.each(function () {
            if ($(this).attr("id") != thisFretID) {
                $(this).css("opacity", "0.3");
            }
        });
    });

    $(document).on("click", ".perde", function () {
        var sayi = $(this).attr("data-numerik");

        var fretboard = $(this).parent().parent();
        var notalar = fretboard.attr("data-notalar");
        var notalarDizi = notalar.split(',');

        if ($(this).hasClass("aktif")) {
            fretboard.find(".perde[data-nota=\"" + $(this).attr("data-nota") + "\"]").removeClass("aktif");

            notalar = "";

            for (var i = 0; i < notalarDizi.length; i++) {
                if (notalarDizi[i] != sayi) {
                    notalar += notalarDizi[i] + ",";
                }
            }

            notalar = notalar.slice(0, -1);
        }
        else {
            if (notalar.length > 0) {
                notalar += "," + sayi;
            }
            else {
                notalar = sayi;
            }

            fretboard.find(".perde[data-nota=\"" + $(this).attr("data-nota") + "\"]").addClass("aktif");
        }

        fretboard.attr("data-notalar", notalar);
    });

    $(document).on("change", ".numerik input[type='checkbox']", function () {
        NumerikYap($(this).is(":checked"));
    });

    $(document).on("change", ".bemol input[type='checkbox']", function () {
        BemolYap($(this).is(":checked"));
    });

    $(document).on("change", ".zoom input[type='checkbox']", function () {
        if ($(this).is(":checked")) {
            $("#fretFrames").addClass("big");
        }
        else {
            $("#fretFrames").removeClass("big");
        }
    });

    $(document).on("click", ".btnBosalt", function () {
        var perdeler = $(this).parent(".kontrol").next(".fretboard").find(".perde");
        var fretboard = $(this).parent(".kontrol").next(".fretboard");
        fretboard.attr("data-notalar", "");

        perdeler.removeClass("aktif");
    });

    $(document).on("click", ".btnKaydet", function () {
        var tip = $(this).attr("data-tip");
        var tipisim = tip == "A" ? "Akor" : "Mod";
        var isim = prompt("Lütfen " + tipisim + " ismi giriniz.");

        if (isim == "") {
            alert("Lütfen önce " + tipisim + " ismi giriniz.");
            return false;
        }

        if (isim != null) {
            var fretboard = $(this).parent(".kontrol").next(".fretboard");
            var notalar = fretboard.children(".tel").first().children(".perde.aktif");
            var numeriknotalar = [];
            var eksenkontrol = 0;
            var i = 0;
            var formul = "";

            notalar.each(function () {
                if ($(this).hasClass("eksen")) {
                    eksenkontrol++;
                }

                if (eksenkontrol == 1) {
                    numeriknotalar[i] = parseInt($(this).attr("data-numerik"));
                    i++;
                }
            });

            if (numeriknotalar.length > 1) {
                for (var i = 1; i < numeriknotalar.length; i++) {
                    var fark = numeriknotalar[i] < numeriknotalar[i - 1] ? (numeriknotalar[i] + 12) - numeriknotalar[i - 1] : numeriknotalar[i] - numeriknotalar[i - 1];

                    formul += fark.toString() + ",";
                }

                formul = formul.slice(0, -1);

                ModAkorKaydet(fretboard, tipisim, isim, formul);
            }
            else {
                alert("Lütfen " + tipisim + " oluşturabilecek içinde eksen ses bulunan bir dizilim oluşturunuz.");
            }
        }
    });

    $(document).on("click", "#ackapat", function () {
        var anakontrol = $(this).parent(".anakontrol");
        var acik = anakontrol.hasClass("acik");

        if (acik) {
            anakontrol.removeClass("acik");
        }
        else {
            anakontrol.addClass("acik");
        }
    });
});

function ModAkorGetir(modakor, nota, tip, notaIsim, tipIsim, fbID, degistir) {
    var ajaxMethod = "";

    if (modakor == "mod") {
        ajaxMethod = "ModDon";
    }
    else if (modakor == "akor") {
        ajaxMethod = "AkorDon";
    }
    else if (modakor == "aralık") {
        ajaxMethod = "AralikDon";
    }

    var fretboardID = "#fretboard" + guid();

    if (tip == "-") {
        alert("Lütfen geçerli bir " + modakor + " seçiniz.");
        return false;
    }

    notaBaslik = "<span class=\"notabaslik\">" + notaIsim + "</span>";

    var tempIsim = tipIsim.split(',');
    var isim = "";

    if (tempIsim.length > 0) {
        for (var i = 0; i < tempIsim.length; i++) {
            if (modakor == "mod") {
                isim += notaBaslik + " " + tempIsim[i] + ", ";
            }
            else if (modakor == "akor") {
                isim += notaBaslik + tempIsim[i] + ", ";
            }
            else if (modakor == "aralık") {
                isim += notaBaslik + " " + tempIsim[i] + ", ";
            }
        }

        isim = isim.slice(0, -2);
    }
    else {
        if (modakor == "mod") {
            isim = notaBaslik + " " + tipIsim;
        }
        else if (modakor == "akor") {
            isim = notaBaslik + tipIsim;
        }
        else if (modakor == "aralık") {
            isim = notaBaslik + " " + tipIsim;
        }
    }

    SessionKontrol().success(function (data) {
        var fretframe = "<div class=\"fretframe\"><div class=\"kontrol\"><b class=\"baslik\">" + isim + "</b>";

        if (data == true) {
            fretframe += "<input class=\"btnKaydet\" type=\"button\" value=\"K (A)\" data-tip=\"A\" title=\"Kaydet (Akor)\" />";
            fretframe += "<input class=\"btnKaydet\" type=\"button\" value=\"K (M)\" data-tip=\"M\" title=\"Kaydet (Mod)\" />";
        }

        fretframe += "<input class=\"btnAra\" type=\"button\" value=\"A\" title=\"Ara\" />";
        fretframe += "<input class=\"btnDigerKaldir\" type=\"button\" value=\"DS\" title=\"Diğerlerini Sil\" />";
        fretframe += "<input class=\"btnKaldir\" type=\"button\" value=\"S\" title=\"Sil\" />";
        fretframe += "<input class=\"btnBosalt\" type=\"button\" value=\"B\" title=\"Boşalt\" />";
        fretframe += "<input class=\"btnOdaklan\" type=\"button\" value=\"O\" title=\"Odaklan\" />";
        fretframe += "<input class=\"btnGizle\" type=\"button\" value=\"G\" title=\"Gizle\" />";
        fretframe += "<input class=\"btnCal\" type=\"button\" value=\"Ç\" title=\"Çal\" />";
        fretframe += "<input class=\"btnTranspoze\" data-yon=\"-\" type=\"button\" value=\"T (-)\" title=\"Transpoze (-)\" />";
        fretframe += "<input class=\"btnTranspoze\" data-yon=\"+\" type=\"button\" value=\"T (+)\" title=\"Transpoze (+)\" />";
        fretframe += "<br /><br />";
        fretframe += "<input class=\"btnYerlestir ileri\" data-yon=\">\" type=\"button\" value=\">\" title=\"Yer Değiştir (İleri)\" />";
        fretframe += "<input class=\"btnYerlestir geri\" data-yon=\"<\" type=\"button\" value=\"<\" title=\"Yer Değiştir (Geri)\" />";
        fretframe += "<input class=\"btnDegistir Aralik\" type=\"button\" value=\"D (I)\" title=\"Değiştir (Aralık)\" />";
        fretframe += "<input class=\"btnDegistir Akor\" type=\"button\" value=\"D (A)\" title=\"Değiştir (Akor)\" />";
        fretframe += "<input class=\"btnDegistir Mod\" type=\"button\" value=\"D (M)\" title=\"Değiştir (Mod)\" />";
        fretframe += "<input class=\"btnEkle Aralik\" type=\"button\" value=\"E (I)\" title=\"Ekle (Aralık)\" />";
        fretframe += "<input class=\"btnEkle Akor\" type=\"button\" value=\"E (A)\" title=\"Ekle (Akor)\" />";
        fretframe += "<input class=\"btnEkle Mod\" type=\"button\" value=\"E (M)\" title=\"Ekle (Mod)\" />";

        if (modakor == "mod") {
            fretframe += "<input class=\"btnAkor\" type=\"button\" data-tip=\"6\" value=\"A (6)\" title=\"Bağlı Hektachordlar\">";
            fretframe += "<input class=\"btnAkor\" type=\"button\" data-tip=\"5\" value=\"A (5)\" title=\"Bağlı Pentachordlar\">";
            fretframe += "<input class=\"btnAkor\" type=\"button\" data-tip=\"4\" value=\"A (4)\" title=\"Bağlı Tetrachordlar\">";
            fretframe += "<input class=\"btnAkor\" type=\"button\" data-tip=\"3\" value=\"A (3)\" title=\"Bağlı Triadlar\">";
            fretframe += "<input class=\"btnMod\" type=\"button\" value=\"BM\" title=\"Bağlı Modlar\">";
        }

        fretframe += "<span class=\"notalar\"></span>";
        fretframe += "</div><div data-nota=\"" + nota + "\" data-tip=\"" + modakor + "\" data-tip-id=\"" + tip + "\" id=\"" + fretboardID + "\">";
        fretframe += "</div></div>";

        if (fbID == undefined) {
            $("#fretFrames").append(fretframe);
        }
        else {
            $(document.getElementById(fbID)).parent(".fretframe").after(fretframe);

            if (degistir == true) {
                $(document.getElementById(fbID)).parent(".fretframe").remove();
            }
        }

        var fretboard = $(document.getElementById(fretboardID));

        fretboard.addClass("fretboard");

        var html = "";

        NotaDoldurAjax().success(function (tempnotalar) {
            for (var i = 0; i < 6; i++) {
                var notalar = NotaDoldur(tempnotalar, i + 1);

                html = "";
                html += "<div class=\"tel\">";
                html += "<div data-num=\"1\" data-notaid=\"" + notalar[0].ID + "\" data-numerik=\"" + notalar[0].Sayisal + "\" data-nota=\"" + notalar[0].Diyezli + "\" data-bemol=\"" + notalar[0].Bemollu + "\" class=\"perde\">" + notalar[0].Diyezli + "</div>";
                html += "<div data-num=\"2\" data-notaid=\"" + notalar[1].ID + "\" data-numerik=\"" + notalar[1].Sayisal + "\" data-nota=\"" + notalar[1].Diyezli + "\" data-bemol=\"" + notalar[1].Bemollu + "\" class=\"perde\">" + notalar[1].Diyezli + "</div>";
                html += "<div data-num=\"3\" data-notaid=\"" + notalar[2].ID + "\" data-numerik=\"" + notalar[2].Sayisal + "\" data-nota=\"" + notalar[2].Diyezli + "\" data-bemol=\"" + notalar[2].Bemollu + "\" class=\"perde\">" + notalar[2].Diyezli + "</div>";
                html += "<div data-num=\"4\" data-notaid=\"" + notalar[3].ID + "\" data-numerik=\"" + notalar[3].Sayisal + "\" data-nota=\"" + notalar[3].Diyezli + "\" data-bemol=\"" + notalar[3].Bemollu + "\" class=\"perde\">" + notalar[3].Diyezli + "</div>";
                html += "<div data-num=\"5\" data-notaid=\"" + notalar[4].ID + "\" data-numerik=\"" + notalar[4].Sayisal + "\" data-nota=\"" + notalar[4].Diyezli + "\" data-bemol=\"" + notalar[4].Bemollu + "\" class=\"perde\">" + notalar[4].Diyezli + "</div>";
                html += "<div data-num=\"6\" data-notaid=\"" + notalar[5].ID + "\" data-numerik=\"" + notalar[5].Sayisal + "\" data-nota=\"" + notalar[5].Diyezli + "\" data-bemol=\"" + notalar[5].Bemollu + "\" class=\"perde\">" + notalar[5].Diyezli + "</div>";
                html += "<div data-num=\"7\" data-notaid=\"" + notalar[6].ID + "\" data-numerik=\"" + notalar[6].Sayisal + "\" data-nota=\"" + notalar[6].Diyezli + "\" data-bemol=\"" + notalar[6].Bemollu + "\" class=\"perde\">" + notalar[6].Diyezli + "</div>";
                html += "<div data-num=\"8\" data-notaid=\"" + notalar[7].ID + "\" data-numerik=\"" + notalar[7].Sayisal + "\" data-nota=\"" + notalar[7].Diyezli + "\" data-bemol=\"" + notalar[7].Bemollu + "\" class=\"perde\">" + notalar[7].Diyezli + "</div>";
                html += "<div data-num=\"9\" data-notaid=\"" + notalar[8].ID + "\" data-numerik=\"" + notalar[8].Sayisal + "\" data-nota=\"" + notalar[8].Diyezli + "\" data-bemol=\"" + notalar[8].Bemollu + "\" class=\"perde\">" + notalar[8].Diyezli + "</div>";
                html += "<div data-num=\"10\" data-notaid=\"" + notalar[9].ID + "\" data-numerik=\"" + notalar[9].Sayisal + "\" data-nota=\"" + notalar[9].Diyezli + "\" data-bemol=\"" + notalar[9].Bemollu + "\" class=\"perde\">" + notalar[9].Diyezli + "</div>";
                html += "<div data-num=\"11\" data-notaid=\"" + notalar[10].ID + "\" data-numerik=\"" + notalar[10].Sayisal + "\" data-nota=\"" + notalar[10].Diyezli + "\" data-bemol=\"" + notalar[10].Bemollu + "\" class=\"perde\">" + notalar[10].Diyezli + "</div>";
                html += "<div data-num=\"12\" data-notaid=\"" + notalar[11].ID + "\" data-numerik=\"" + notalar[11].Sayisal + "\" data-nota=\"" + notalar[11].Diyezli + "\" data-bemol=\"" + notalar[11].Bemollu + "\" class=\"perde\">" + notalar[11].Diyezli + "</div>";
                html += "<div data-num=\"13\" data-notaid=\"" + notalar[0].ID + "\" data-numerik=\"" + notalar[0].Sayisal + "\" data-nota=\"" + notalar[0].Diyezli + "\" data-bemol=\"" + notalar[0].Bemollu + "\" class=\"perde\">" + notalar[0].Diyezli + "</div>";
                html += "<div data-num=\"14\" data-notaid=\"" + notalar[1].ID + "\" data-numerik=\"" + notalar[1].Sayisal + "\" data-nota=\"" + notalar[1].Diyezli + "\" data-bemol=\"" + notalar[1].Bemollu + "\" class=\"perde\">" + notalar[1].Diyezli + "</div>";
                html += "<div data-num=\"15\" data-notaid=\"" + notalar[2].ID + "\" data-numerik=\"" + notalar[2].Sayisal + "\" data-nota=\"" + notalar[2].Diyezli + "\" data-bemol=\"" + notalar[2].Bemollu + "\" class=\"perde\">" + notalar[2].Diyezli + "</div>";
                html += "<div data-num=\"16\" data-notaid=\"" + notalar[3].ID + "\" data-numerik=\"" + notalar[3].Sayisal + "\" data-nota=\"" + notalar[3].Diyezli + "\" data-bemol=\"" + notalar[3].Bemollu + "\" class=\"perde\">" + notalar[3].Diyezli + "</div>";
                html += "<div data-num=\"17\" data-notaid=\"" + notalar[4].ID + "\" data-numerik=\"" + notalar[4].Sayisal + "\" data-nota=\"" + notalar[4].Diyezli + "\" data-bemol=\"" + notalar[4].Bemollu + "\" class=\"perde\">" + notalar[4].Diyezli + "</div>";
                html += "<div data-num=\"18\" data-notaid=\"" + notalar[5].ID + "\" data-numerik=\"" + notalar[5].Sayisal + "\" data-nota=\"" + notalar[5].Diyezli + "\" data-bemol=\"" + notalar[5].Bemollu + "\" class=\"perde\">" + notalar[5].Diyezli + "</div>";
                html += "<div data-num=\"19\" data-notaid=\"" + notalar[6].ID + "\" data-numerik=\"" + notalar[6].Sayisal + "\" data-nota=\"" + notalar[6].Diyezli + "\" data-bemol=\"" + notalar[6].Bemollu + "\" class=\"perde\">" + notalar[6].Diyezli + "</div>";
                html += "<div data-num=\"20\" data-notaid=\"" + notalar[7].ID + "\" data-numerik=\"" + notalar[7].Sayisal + "\" data-nota=\"" + notalar[7].Diyezli + "\" data-bemol=\"" + notalar[7].Bemollu + "\" class=\"perde\">" + notalar[7].Diyezli + "</div>";
                html += "<div data-num=\"21\" data-notaid=\"" + notalar[8].ID + "\" data-numerik=\"" + notalar[8].Sayisal + "\" data-nota=\"" + notalar[8].Diyezli + "\" data-bemol=\"" + notalar[8].Bemollu + "\" class=\"perde\">" + notalar[8].Diyezli + "</div>";
                html += "<div data-num=\"22\" data-notaid=\"" + notalar[9].ID + "\" data-numerik=\"" + notalar[9].Sayisal + "\" data-nota=\"" + notalar[9].Diyezli + "\" data-bemol=\"" + notalar[9].Bemollu + "\" class=\"perde\">" + notalar[9].Diyezli + "</div>";
                html += "<div data-num=\"23\" data-notaid=\"" + notalar[10].ID + "\" data-numerik=\"" + notalar[10].Sayisal + "\" data-nota=\"" + notalar[10].Diyezli + "\" data-bemol=\"" + notalar[10].Bemollu + "\" class=\"perde\">" + notalar[10].Diyezli + "</div>";
                html += "<div data-num=\"24\" data-notaid=\"" + notalar[11].ID + "\" data-numerik=\"" + notalar[11].Sayisal + "\" data-nota=\"" + notalar[11].Diyezli + "\" data-bemol=\"" + notalar[11].Bemollu + "\" class=\"perde\">" + notalar[11].Diyezli + "</div>";
                html += "<div data-num=\"25\" data-notaid=\"" + notalar[0].ID + "\" data-numerik=\"" + notalar[0].Sayisal + "\" data-nota=\"" + notalar[0].Diyezli + "\" data-bemol=\"" + notalar[0].Bemollu + "\" class=\"perde\">" + notalar[0].Diyezli + "</div>";
                html += "</div>";

                fretboard.append(html);

                $.ajax({
                    type: "POST",
                    url: MainPath + "/Ajax/" + ajaxMethod,
                    data: "{nota: " + nota + ", " + modakor + ": " + tip + "}",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (result) {
                        fretboard.children("div.tel").children("div.perde").removeClass("aktif");
                        fretboard.children("div.tel").children("div.perde").removeClass("eksen");
                        fretboard.children("div.tel").children("div.perde[data-numerik=\"" + nota + "\"]").addClass("eksen");

                        var notalar = "";
                        var notalarid = "";

                        if (result != null) {
                            for (var i = 0; i < result.length; i++) {
                                fretboard.children("div.tel").children("div.perde[data-nota=\"" + result[i] + "\"]").addClass("aktif");
                                notalar += result[i] + " - ";
                                notalarid += result[i] + ",";
                            }

                            fretboard.prev(".kontrol").children(".notalar").text("(" + notalar.slice(0, -3) + ")");
                            notalarid = notalarid.slice(0, -1);

                            var tempnotalarid = "";
                            var tel = fretboard.children(".tel").first();

                            for (var i = 0; i < notalarid.split(',').length; i++) {
                                tempnotalarid += tel.children(".perde[data-nota=\"" + notalarid.split(',')[i] + "\"]").first().attr("data-notaid") + ",";
                            }

                            notalarid = tempnotalarid.slice(0, -1);

                            fretboard.attr("data-notalar", notalarid);

                            var isNum = $(".numerik input[type='checkbox']").is(":checked");
                            var isBemol = $(".bemol input[type='checkbox']").is(":checked");

                            if (isNum) {
                                NumerikYap(true, fretboard);
                            }

                            if (isBemol) {
                                BemolYap(true, fretboard);
                            }
                        }
                    }
                });
            }
        });
    });
}

function ModAkorKaydet(fretboard, tipisim, isim, formul) {
    ModAkorKaydetAjax(tipisim, isim, formul).success(function (sonuc) {
        if (sonuc == -1) {
            alert("Bu formülde bir " + tipisim + " zaten daha önce kaydedilmiş.");
        }
        else if (sonuc == 0) {
            alert("Kayıt sırasında bir hata meydana geldi.");
        }
        else if (sonuc > 0) {
            alert("Kayıt başarıyla gerçekleşti.");
            var combo = $("#cmb" + tipisim + "lar");
            ComboDoldur(tipisim, combo, true).success(function () {
                combo.val(sonuc);

                fretboard.prev(".kontrol").find(".btnDegistir." + tipisim).click();
            });
        }
    });
}

function BagliAkorlar(tip, nota, modid) {
    return $.ajax({
        type: "POST",
        url: MainPath + "/Ajax/AkorOlustur",
        dataType: "json",
        data: "{tip: " + tip + ", nota: " + nota + ", modid: " + modid + "}",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.length <= 0) {
                alert("Bağlı akor bulunamadı.");
                return false;
            }

            $.each(result, function (i, data) {
                ModAkorGetir("akor", data.Nota, data.ID, data.NotaIsim, data.Isim, undefined, undefined);
            });

            $("#loader").removeClass("acik");
        }
    });
}

function BagliModlar(nota, modid) {
    return $.ajax({
        type: "POST",
        url: MainPath + "/Ajax/ModOlustur",
        dataType: "json",
        data: "{nota: " + nota + ", modid: " + modid + "}",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.length <= 0) {
                alert("Bağlı mod bulunamadı.");
                return false;
            }

            $.each(result, function (i, data) {
                ModAkorGetir("mod", data.Nota, data.ID, data.NotaIsim, data.Isim, undefined, undefined);
            });

            $("#loader").removeClass("acik");
        }
    });
}

function NotaDoldur(tempNotalar, tel) {
    var notalar = [12];

    for (j = 0; j < 12; j++) {
        if (tel == 1 || tel == 6) {
            notalar[j] = tempNotalar[j + 4];
        }
        else if (tel == 2) {
            notalar[j] = tempNotalar[j + 11];
        }
        else if (tel == 3) {
            notalar[j] = tempNotalar[j + 7];
        }
        else if (tel == 4) {
            notalar[j] = tempNotalar[j + 2];
        }
        else if (tel == 5) {
            notalar[j] = tempNotalar[j + 9];
        }
    }

    return notalar;
}

function NotaDoldurAjax() {
    return $.ajax({
        type: "POST",
        url: MainPath + "/Ajax/TempNota",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
    });
}

function ModAkorKaydetAjax(tipisim, isim, formul) {
    return $.ajax({
        type: "POST",
        url: MainPath + "/Ajax/" + tipisim + "Kaydet",
        dataType: "json",
        data: "{isim: '" + isim + "', formul: '" + formul + "'}",
        contentType: "application/json; charset=utf-8",
    });
}

function ComboDoldur(comboTip, comboItem, bosVarMi) {
    comboItem.html("");

    return $.ajax({
        type: "POST",
        url: MainPath + "/Ajax/" + comboTip + "Combo",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (bosVarMi == true) {
                comboItem.append("<option value=\"-\">- " + comboTip + " -</option>");
            }

            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    var isim = result[i].Isim;
                    var nota = "";

                    if (isim == undefined) {
                        isim = result[i].Diyezli;
                        nota = " data-num=\"" + result[i].Sayisal + "\" data-bemol=\"" + result[i].Bemollu + "\" data-diyez=\"" + result[i].Diyezli + "\"";
                    }

                    comboItem.append("<option" + nota + " value=\"" + result[i].ID + "\">" + isim + "</option>");
                }
            }
        }
    });
}

function FretboardTemizle() {
    $(".fretframe").fadeOut("slow", function () {
        $(this).remove();
    });
}

function GirisKontrol(sifre) {
    $.ajax({
        type: "POST",
        url: MainPath + "/Ajax/Giris",
        data: "{sifre: '" + sifre + "'}",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (sonuc) {
            if (sonuc == true) {
                window.location.href = MainPath;
            }
            else {
                alert("Giriş Başarısız.");
            }
        }
    });
}

function SessionKontrol() {
    return $.ajax({
        type: "POST",
        url: MainPath + "/Ajax/SessionKontrol",
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}

function NumerikYap(yap, fretboard) {
    var perde;
    var baslik;
    var notalar;

    if (fretboard == undefined) {
        perde = $(".fretboard .tel .perde");
        baslik = $(".fretboard").prev(".kontrol").children(".baslik").children(".notabaslik");
        notalar = $(".fretboard").prev(".kontrol").children(".notalar");
    }
    else {
        perde = fretboard.find(".perde");
        baslik = fretboard.prev(".kontrol").children(".baslik").children(".notabaslik");
        notalar = fretboard.prev(".kontrol").children(".notalar");
    }

    combo = $("#cmbNotalar").children("option");

    if (yap) {
        $("#chkBemol").attr("disabled", "disabled");

        perde.each(function () {
            $(this).text($(this).attr("data-numerik"));
        });

        baslik.each(function () {
            var yenibaslik = $(".fretboard .tel .perde[data-nota='" + $(this).text() + "']").first().attr("data-numerik");
            $(this).text(yenibaslik);
        });

        notalar.each(function () {
            var tempnotalar = $(this).text().replace("(", "").replace(")", "").split('-');
            var yeninotalar = "(";

            for (var i = 0; i < tempnotalar.length; i++) {
                var yenibaslik = $(".fretboard .tel .perde[data-nota='" + tempnotalar[i].replace(" ", "").replace(" ", "") + "']").first().attr("data-numerik");
                yeninotalar += " " + yenibaslik + " -";
            }

            yeninotalar = yeninotalar.replace("( ", "(").slice(0, -2) + ")";

            $(this).text(yeninotalar);
        });

        combo.each(function () {
            $(this).text($(this).attr("data-num"));
        });
    }
    else {
        $("#chkBemol").removeAttr("disabled");

        perde.each(function () {
            $(this).text($(this).attr("data-nota"));
        });

        baslik.each(function () {
            var yenibaslik = $(".fretboard .tel .perde[data-numerik='" + $(this).text() + "']").first().attr("data-nota");
            $(this).text(yenibaslik);
        });

        notalar.each(function () {
            var tempnotalar = $(this).text().replace("(", "").replace(")", "").split('-');
            var yeninotalar = "(";

            for (var i = 0; i < tempnotalar.length; i++) {
                var yenibaslik = $(".fretboard .tel .perde[data-numerik='" + tempnotalar[i].replace(" ", "").replace(" ", "") + "']").first().attr("data-nota");
                yeninotalar += " " + yenibaslik + " -";
            }

            yeninotalar = yeninotalar.replace("( ", "(").slice(0, -2) + ")";

            $(this).text(yeninotalar);
        });

        combo.each(function () {
            $(this).text($(this).attr("data-diyez"));
        });
    }
}

function BemolYap(yap, fretboard) {
    var perde;
    var baslik;
    var notalar;
    var combo;

    if (fretboard == undefined) {
        perde = $(".fretboard .tel .perde");
        baslik = $(".fretboard").prev(".kontrol").children(".baslik").children(".notabaslik");
        notalar = $(".fretboard").prev(".kontrol").children(".notalar");
    }
    else {
        perde = fretboard.find(".perde");
        baslik = fretboard.prev(".kontrol").children(".baslik").children(".notabaslik");
        notalar = fretboard.prev(".kontrol").children(".notalar");
    }

    combo = $("#cmbNotalar").children("option");

    if (yap) {
        $("#chkNumerik").attr("disabled", "disabled");

        perde.each(function () {
            $(this).text($(this).attr("data-bemol"));
        });

        baslik.each(function () {
            var yenibaslik = $(".fretboard .tel .perde[data-nota='" + $(this).text() + "']").first().attr("data-bemol");
            $(this).text(yenibaslik);
        });

        notalar.each(function () {
            var tempnotalar = $(this).text().replace("(", "").replace(")", "").split('-');
            var yeninotalar = "(";

            for (var i = 0; i < tempnotalar.length; i++) {
                var yenibaslik = $(".fretboard .tel .perde[data-nota='" + tempnotalar[i].replace(" ", "").replace(" ", "") + "']").first().attr("data-bemol");
                yeninotalar += " " + yenibaslik + " -";
            }

            yeninotalar = yeninotalar.replace("( ", "(").slice(0, -2) + ")";

            $(this).text(yeninotalar);
        });

        combo.each(function () {
            $(this).text($(this).attr("data-bemol"));
        });
    }
    else {
        $("#chkNumerik").removeAttr("disabled");

        perde.each(function () {
            $(this).text($(this).attr("data-nota"));
        });

        baslik.each(function () {
            var yenibaslik = $(".fretboard .tel .perde[data-bemol='" + $(this).text() + "']").first().attr("data-nota");
            $(this).text(yenibaslik);
        });

        notalar.each(function () {
            var tempnotalar = $(this).text().replace("(", "").replace(")", "").split('-');
            var yeninotalar = "(";

            for (var i = 0; i < tempnotalar.length; i++) {
                var yenibaslik = $(".fretboard .tel .perde[data-bemol='" + tempnotalar[i].replace(" ", "").replace(" ", "") + "']").first().attr("data-nota");
                yeninotalar += " " + yenibaslik + " -";
            }

            yeninotalar = yeninotalar.replace("( ", "(").slice(0, -2) + ")";

            $(this).text(yeninotalar);
        });

        combo.each(function () {
            $(this).text($(this).attr("data-diyez"));
        });
    }
}

function NotaDon(sayi) {
    switch (sayi) {
        case "1": "C"; break;
        case "2": "C#"; break;
        case "3": "D"; break;
        case "4": "D#"; break;
        case "5": "E"; break;
        case "6": "F"; break;
        case "7": "F#"; break;
        case "8": "G"; break;
        case "9": "G#"; break;
        case "10": "A"; break;
        case "11": "A#"; break;
        case "12": "B"; break;
    }
}

function SayiDon(nota) {
    switch (nota) {
        case "C": "1"; break;
        case "C#": "2"; break;
        case "D": "3"; break;
        case "D#": "4"; break;
        case "E": "5"; break;
        case "F": "6"; break;
        case "F#": "7"; break;
        case "G": "8"; break;
        case "G#": "9"; break;
        case "A": "10"; break;
        case "A#": "11"; break;
        case "B": "12"; break;
    }
}

function AkorSirala() {
    var f = $(".fretframe")[0]
    var notalar = f.children[1].getAttribute("data-notalar").split(',');
    var grup = new Array();

    $.each(notalar, function (i) {
        var fb = $(".fretframe > .fretboard[data-nota=\'" + notalar[i] + "\'][data-notalar!=\'" + f.children[1].getAttribute("data-notalar") + "\']");
        grup[i] = fb.parent(".fretframe");

        $.each(fb.parent(".fretframe"), function (j) {
            $(this).remove();
        });
    });

    for (var i = 0; i < grup.length; i++) {
        for (var j = 0; j < grup[i].length; j++) {
            grup[i].sort(function (a, b) {
                return parseFloat($(a).children(".fretboard").attr("data-tip-id")) - parseFloat($(b).children(".fretboard").attr("data-tip-id"));
            });
        }
    }

    $.each(grup, function (j) {
        $.each(grup[j], function (k) {
            $(f).parent().append($(this));
        });
    });
}

function Sirala(notalar) {
    var notalarDizi = notalar.split(',');
    notalarDizi.sort(function (a, b) { return a - b });
    notalar = "";

    for (var i = 0; i < notalarDizi.length; i++) {
        notalar += notalarDizi[i].toString() + ",";
    }

    return notalar.slice(0, -1);
}

function FormulDon(tel) {
    var eksennota = tel.find(".eksen").first();
    var eksen = tel.find(".eksen").first().attr("data-numerik");

    if (!eksennota.hasClass("aktif")) {
        alert("Lütfen önce eksen ses seçiniz.");
        return false;
    }

    var perdeler = tel.find(".perde.aktif");
    var eksenilk = false;
    var eksenson = false;

    var notalar = new Array();
    var j = 0;

    perdeler.each(function (i, data) {
        if (eksenilk == false) {
            if (data.attributes[2].value == eksen) {
                notalar[j] = parseInt(data.attributes[2].value);
                eksenilk = true;
                j++;
            }
        }
        else {
            if (eksenson == false) {
                if (data.attributes[2].value == eksen) {
                    eksenson = true;
                }
                else {
                    notalar[j] = parseInt(data.attributes[2].value);
                    j++;
                }
            }
        }
    });

    var formul = "";

    for (var i = 0; i < notalar.length; i++) {
        if (i > 0) {
            if (notalar[i] < notalar[i - 1]) {
                notalar[i] = notalar[i] + 12;
            }

            formul += (notalar[i] - notalar[i - 1]).toString() + ",";
        }
    }

    return formul.slice(0, -1);
}

function guid() {
    return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
}