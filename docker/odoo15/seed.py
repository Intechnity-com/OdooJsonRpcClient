# Seed script executed via `odoo shell` (variable `env` is provided by the shell).
# Prepares data expected by PortaCapena.OdooJsonRpcClient.Example tests + some extra data to play with.
import logging
from datetime import timedelta

from odoo import fields

_log = logging.getLogger("seed")


def section(name):
    def wrap(fn):
        try:
            with env.cr.savepoint():
                fn()
            print(f"[seed] OK   {name}")
        except Exception as e:  # keep seeding other sections
            print(f"[seed] FAIL {name}: {e!r}")
        return fn
    return wrap


admin = env.ref("base.user_admin")
main_company = env.ref("base.main_company")
Company = env["res.company"]


def ensure_company(name, country_code, currency_code):
    company = Company.search([("name", "=", name)], limit=1)
    country = env["res.country"].search([("code", "=", country_code)], limit=1)
    currency = env["res.currency"].with_context(active_test=False).search([("name", "=", currency_code)], limit=1)
    currency.active = True
    vals = {"name": name, "country_id": country.id, "currency_id": currency.id}
    if company:
        company.write(vals)
    else:
        company = Company.create(vals)
    admin.company_ids |= company
    # chart of accounts (generic) so taxes/journals exist for the company
    if not env["account.account"].search_count([("company_id", "=", company.id)]):
        chart = env.ref("l10n_generic_coa.configurable_chart_template", raise_if_not_found=False)
        if chart:
            chart.try_loading(company=company, install_demo=False)
    return company


@section("companies")
def _companies():
    main_company.write({
        "name": "My Company (San Francisco)",
        "country_id": env.ref("base.us").id,
    })
    ensure_company("PL Company", "PL", "PLN")
    ensure_company("My Company (Chicago)", "US", "USD")
    ensure_company("BE Company", "BE", "EUR")


@section("currencies")
def _currencies():
    for code in ("EUR", "PLN", "USD", "GBP"):
        env["res.currency"].with_context(active_test=False).search([("name", "=", code)]).active = True


@section("admin groups (multi-company, multi-currency, lots, variants, uom, pricelists)")
def _groups():
    xmlids = [
        "base.group_multi_company",
        "base.group_multi_currency",
        "product.group_product_variant",
        "product.group_product_pricelist",
        "uom.group_uom",
        "stock.group_production_lot",
        "stock.group_stock_multi_locations",
        "stock.group_stock_multi_warehouses",
        "sale.group_discount_per_so_line",
    ]
    groups = env["res.groups"]
    for x in xmlids:
        g = env.ref(x, raise_if_not_found=False)
        if g:
            groups |= g
    # add to "Internal User" so every internal user (incl. admin) gets them
    env.ref("base.group_user").implied_ids |= groups
    admin.groups_id |= groups


@section("partners")
def _partners():
    Partner = env["res.partner"]
    data = [
        ("Test Partner PL", "PL", "Warszawa", "PL5260250995"),
        ("Test Partner BE", "BE", "Brussels", "BE0477472701"),
        ("Test Partner US", "US", "New York", False),
    ]
    for name, cc, city, vat in data:
        if not Partner.search_count([("name", "=", name)]):
            Partner.create({
                "name": name,
                "is_company": True,
                "country_id": env["res.country"].search([("code", "=", cc)], limit=1).id,
                "city": city,
                "vat": vat,
                "email": f"{name.lower().replace(' ', '.')}@example.com",
            })


@section("products (lot-tracked, service, consumable)")
def _products():
    Tmpl = env["product.template"]
    if not Tmpl.search_count([("default_code", "=", "TEST-LOT")]):
        tmpl = Tmpl.create({
            "name": "Test Lot Tracked Product",
            "default_code": "TEST-LOT",
            "type": "product",
            "tracking": "lot",
            "list_price": 99.0,
            "standard_price": 50.0,
        })
        product = tmpl.product_variant_id
        stock_loc = env.ref("stock.stock_location_stock")
        for i in range(1, 4):
            lot = env["stock.production.lot"].create({
                "name": f"LOT-{i:03d}",
                "product_id": product.id,
                "company_id": main_company.id,
            })
            env["stock.quant"]._update_available_quantity(product, stock_loc, 10 * i, lot_id=lot)
    if not Tmpl.search_count([("default_code", "=", "TEST-SRV")]):
        Tmpl.create({"name": "Test Service", "default_code": "TEST-SRV", "type": "service", "list_price": 150.0})
    if not Tmpl.search_count([("default_code", "=", "TEST-CONS")]):
        Tmpl.create({"name": "Test Consumable", "default_code": "TEST-CONS", "type": "consu", "list_price": 5.0})


@section("coupon program")
def _coupon():
    Program = env["coupon.program"]
    if not Program.search_count([("name", "=", "Test 10% Discount")]):
        Program.create({
            "name": "Test 10% Discount",
            "program_type": "promotion_program",
            "promo_code_usage": "code_needed",
            "promo_code": "TEST10",
            "reward_type": "discount",
            "discount_type": "percentage",
            "discount_percentage": 10,
        })


@section("sale orders (draft + confirmed)")
def _sales():
    partner = env["res.partner"].search([("name", "=", "Test Partner PL")], limit=1)
    product = env["product.product"].search([("default_code", "=", "TEST-CONS")], limit=1)
    for confirm in (False, True):
        so = env["sale.order"].create({
            "partner_id": partner.id,
            "order_line": [(0, 0, {"product_id": product.id, "product_uom_qty": 3})],
        })
        if confirm:
            so.action_confirm()


@section("purchase orders (draft + confirmed)")
def _purchases():
    partner = env["res.partner"].search([("name", "=", "Test Partner BE")], limit=1)
    product = env["product.product"].search([("default_code", "=", "TEST-LOT")], limit=1)
    for confirm in (False, True):
        po = env["purchase.order"].create({
            "partner_id": partner.id,
            "order_line": [(0, 0, {
                "product_id": product.id,
                "product_qty": 5,
                "price_unit": 45.0,
                "date_planned": fields.Datetime.now() + timedelta(days=7),
            })],
        })
        if confirm:
            po.button_confirm()


@section("customer invoice (posted)")
def _invoice():
    partner = env["res.partner"].search([("name", "=", "Test Partner US")], limit=1)
    product = env["product.product"].search([("default_code", "=", "TEST-SRV")], limit=1)
    move = env["account.move"].create({
        "move_type": "out_invoice",
        "partner_id": partner.id,
        "invoice_line_ids": [(0, 0, {"product_id": product.id, "quantity": 2, "price_unit": 150.0})],
    })
    move.action_post()


env.cr.commit()
print("[seed] committed")
