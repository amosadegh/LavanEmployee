// employees-form.js
// استفاده در Create و Edit پرسنل
// نیاز: jQuery, Select2, Persian Datepicker

var EmployeesForm = (function () {

    // URL های Ajax — از View پاس داده می‌شوند
    var _urls = {
        getSecondOrgs: '',
        getCities: '',
        getEduOrientations: ''
    };

    // مقادیر اولیه (فقط در Edit استفاده می‌شود)
    var _initial = {
        secondOrgId: null,
        birthCityId: null,
        entryCityId: null,
        homeCityId: null,
        eduOrientationId: null
    };

    // ─── Select2 ────
    function initSelect2() {
        $('.select2').select2({
            theme: 'bootstrap-5',
            dir: 'rtl',
            language: 'fa',
            placeholder: 'انتخاب کنید',
            allowClear: true
        });
    }

    // ───── Persian Datepicker────
    function initDatepicker() {
        $('.datepicker').persianDatepicker({
            format: 'YYYY/MM/DD',
            initialValue: false,
            autoClose: true,
            calendar: { persian: { locale: 'fa' } },
            navigator: { enabled: true },
            toolbox: { enabled: true },
            timePicker: { enabled: false }
        });
    }

    // ───── Collapse icon ─────
    function initColapseIcons() {
        $('.card-header[data-bs-toggle="collapse"]').on('click', function () {
            $(this).find('.toggle-icon').toggleClass('rotate-180');
        });
    }

    // ───── جلوگیری از submit با Enter ─────
    function preventEnterSubmit() {
        $('form').on('keypress', function (e) {
            if (e.which === 13 && e.target.tagName !== 'TEXTAREA') {
                e.preventDefault();
                return false;
            }
        });
    }

    // ───── بارگذاری SecondOrganization ─────
    function loadSecondOrgs(orgId, selectedId) {
        var $select = $('#SecondOrganizationId');
        $select.empty().append($('<option>', { value: '', text: 'انتخاب کنید' }));
        $select.trigger('change');

        if (!orgId) return;

        $.getJSON(_urls.getSecondOrgs, { organizationId: orgId }, function (items) {
            $.each(items, function (_, item) {
                $select.append($('<option>', { value: item.id, text: item.title }));
            });
            if (selectedId) $select.val(selectedId);
            $select.trigger('change');
        });
    }

    // ───── بارگذاری شهر بر اساس استان ─────
    function loadCities(provinceId, $citySelect, selectedId) {
        $citySelect.empty().append($('<option>', { value: '', text: 'انتخاب کنید' }));
        $citySelect.trigger('change');

        if (!provinceId) return;

        $.getJSON(_urls.getCities, { provinceId: provinceId }, function (items) {
            $.each(items, function (_, item) {
                $citySelect.append($('<option>', { value: item.id, text: item.name }));
            });
            if (selectedId) $citySelect.val(selectedId);
            $citySelect.trigger('change');
        });
    }

    // ─── بارگذاری گرایش تحصیلی ─────
    function loadEduOrientations(studyFieldId, selectedId) {
        var $select = $('#EduOrientationId');
        $select.empty().append($('<option>', { value: '', text: 'انتخاب کنید' }));
        $select.trigger('change');

        if (!studyFieldId) return;

        $.getJSON(_urls.getEduOrientations, { studyFieldId: studyFieldId }, function (items) {
            $.each(items, function (_, item) {
                $select.append($('<option>', { value: item.id, text: item.title }));
            });
            if (selectedId) $select.val(selectedId);
            $select.trigger('change');
        });
    }

    // ─── wire-up رویدادها ─────
    function bindEvents() {

        // Organization → SecondOrganization
        $('#OrganizationId').on('change', function () {
            loadSecondOrgs($(this).val(), null);
        });

        // BirthProvice → BirthCity   (نام مدل: BirthProvice)
        $('#BirthProvice').on('change', function () {
            loadCities($(this).val(), $('#BirthCity'), null);
        });

        // EntryProviance → EntryCity  (نام مدل: EntryProviance)
        $('#EntryProviance').on('change', function () {
            loadCities($(this).val(), $('#EntryCity'), null);
        });

        // HomeProvince → HomeCity
        $('#HomeProvince').on('change', function () {
            loadCities($(this).val(), $('#HomeCity'), null);
        });

        // StudyFieldId → EduOrientationId
        $('#StudyFieldId').on('change', function () {
            loadEduOrientations($(this).val(), null);
        });
    }

    // ───── بارگذاری اولیه (Edit) ─────
    function loadInitialValues() {
        var orgId = $('#OrganizationId').val();
        if (orgId) loadSecondOrgs(orgId, _initial.secondOrgId);

        var birthProv = $('#BirthProvice').val();
        if (birthProv) loadCities(birthProv, $('#BirthCity'), _initial.birthCityId);

        var entryProv = $('#EntryProviance').val();
        if (entryProv) loadCities(entryProv, $('#EntryCity'), _initial.entryCityId);

        var homeProv = $('#HomeProvince').val();
        if (homeProv) loadCities(homeProv, $('#HomeCity'), _initial.homeCityId);

        var studyField = $('#StudyFieldId').val();
        if (studyField) loadEduOrientations(studyField, _initial.eduOrientationId);
    }

    // ─── init عمومی ─────
    function init(options) {
        _urls.getSecondOrgs = options.urls.getSecondOrgs;
        _urls.getCities = options.urls.getCities;
        _urls.getEduOrientations = options.urls.getEduOrientations;

        if (options.initial) {
            $.extend(_initial, options.initial);
        }

        initSelect2();
        initDatepicker();
        initCollapseIcons();
        preventEnterSubmit();
        bindEvents();
        loadInitialValues();
    }

    return { init: init };

})();
