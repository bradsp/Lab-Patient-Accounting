# Project Issues Log

Non-critical enhancements discovered during execution. Address in future phases when appropriate.

## Open Enhancements

### ISS-001: Client Maintenance — Discounts + Interface Mappings child grids (13-05)
- **Discovered:** Phase 13 planning (2026-07-02)
- **Type:** UX / Feature parity
- **Description:** The legacy `ClientMaintenanceEditForm` includes two child grids deferred out of the
  initial Client dictionary maintenance build: (1) per-client CDM **Discounts** — an editable grid with
  CDM lookup and live price calculation from fee schedules (percent↔price recompute using
  `CdmDetail`/`CdmFeeSchedule` pricing), persisted via `ClientDiscountRepository.SaveAll` /
  `DictionaryService.SaveClient` (`client.Discounts`); and (2) **Interface Mappings** — the client's
  external-system aliases (`Mapping`, type "CLIENT"), read via
  `DictionaryService.GetMappingsBySendingValue`. Both were intentionally deferred to ship core client
  maintenance first (decision recorded in Phase 13 planning).
- **Impact:** Low (core client maintenance is fully functional without these; they are less-frequently
  edited than scalar fields).
- **Effort:** Substantial (>4hr — CDM lookup component, price-calc logic, two editable grids, save wiring).
- **Suggested phase:** Phase 13, Plan 05 (`13-05-PLAN.md`) — follow-up to the four core plans.

## Closed Enhancements

_None yet._

---

**Summary:** 1 open, 0 closed
**Priority queue:** ISS-001 (address as a 13-05 follow-up after core Client maintenance ships)
