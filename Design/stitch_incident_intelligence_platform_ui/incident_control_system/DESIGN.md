---
name: Incident Control System
colors:
  surface: '#f9f9ff'
  surface-dim: '#d0daf2'
  surface-bright: '#f9f9ff'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#f0f3ff'
  surface-container: '#e8eeff'
  surface-container-high: '#dfe8ff'
  surface-container-highest: '#d9e3fb'
  on-surface: '#111c2d'
  on-surface-variant: '#424753'
  inverse-surface: '#273143'
  inverse-on-surface: '#ecf0ff'
  outline: '#727784'
  outline-variant: '#c2c6d5'
  surface-tint: '#0159c5'
  primary: '#0056bd'
  on-primary: '#ffffff'
  primary-container: '#2e6fdb'
  on-primary-container: '#faf9ff'
  inverse-primary: '#aec6ff'
  secondary: '#525f73'
  on-secondary: '#ffffff'
  secondary-container: '#d6e3fb'
  on-secondary-container: '#586579'
  tertiary: '#006839'
  on-tertiary: '#ffffff'
  tertiary-container: '#00844a'
  on-tertiary-container: '#eaffeb'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#d8e2ff'
  primary-fixed-dim: '#aec6ff'
  on-primary-fixed: '#001a43'
  on-primary-fixed-variant: '#004397'
  secondary-fixed: '#d6e3fb'
  secondary-fixed-dim: '#bac7de'
  on-secondary-fixed: '#0f1c2d'
  on-secondary-fixed-variant: '#3b485a'
  tertiary-fixed: '#70fda7'
  tertiary-fixed-dim: '#51df8e'
  on-tertiary-fixed: '#00210e'
  on-tertiary-fixed-variant: '#00522c'
  background: '#f9f9ff'
  on-background: '#111c2d'
  surface-variant: '#d9e3fb'
typography:
  headline-xl:
    fontFamily: Inter
    fontSize: 32px
    fontWeight: '600'
    lineHeight: 40px
    letterSpacing: -0.02em
  headline-lg:
    fontFamily: Inter
    fontSize: 24px
    fontWeight: '600'
    lineHeight: 32px
    letterSpacing: -0.015em
  headline-md:
    fontFamily: Inter
    fontSize: 20px
    fontWeight: '600'
    lineHeight: 28px
    letterSpacing: -0.01em
  headline-sm:
    fontFamily: Inter
    fontSize: 16px
    fontWeight: '600'
    lineHeight: 24px
  body-lg:
    fontFamily: Inter
    fontSize: 16px
    fontWeight: '400'
    lineHeight: 24px
  body-md:
    fontFamily: Inter
    fontSize: 14px
    fontWeight: '400'
    lineHeight: 20px
  body-sm:
    fontFamily: Inter
    fontSize: 12px
    fontWeight: '400'
    lineHeight: 16px
  label-md:
    fontFamily: Inter
    fontSize: 13px
    fontWeight: '600'
    lineHeight: 18px
  label-sm:
    fontFamily: Inter
    fontSize: 11px
    fontWeight: '600'
    lineHeight: 16px
    letterSpacing: 0.02em
  code-md:
    fontFamily: JetBrains Mono
    fontSize: 13px
    fontWeight: '400'
    lineHeight: 20px
  code-sm:
    fontFamily: JetBrains Mono
    fontSize: 11px
    fontWeight: '400'
    lineHeight: 16px
rounded:
  sm: 0.125rem
  DEFAULT: 0.25rem
  md: 0.375rem
  lg: 0.5rem
  xl: 0.75rem
  full: 9999px
spacing:
  gutter: 1rem
  gutter-lg: 1.5rem
  margin: 1.5rem
  margin-sm: 1rem
  space-xs: 0.5rem
  space-sm: 1rem
  space-md: 1.5rem
  space-lg: 2rem
  space-xl: 3rem
---

## Brand & Style

This design system is engineered for high-pressure incident response and observability workflows. The aesthetic reflects a modern, calm control room: austere, hyper-legible, and uncompromisingly functional. The core priority is immediate cognitive processing under duress. 

Visual decoration, skeuomorphic depth, gratuitous gradients, and heavy shadows are entirely absent. Structural division relies on crisp 1px borders, rigid grid alignments, and high-contrast typographic hierarchy. The system engenders composure, precision, and operational authority, allowing Site Reliability Engineers (SREs) and on-call responders to isolate signals from noise instantly.

## Colors

The palette balances clinical clarity with functional status accents. Interfaces rely strictly on white and cool neutral background planes, preserving saturated colors solely for state conveyance and interactive affordances.

### Primary & Action
- **Primary Base**: `#2E6FDB` — Applied to primary interactions, selected states, and focused boundaries.
- **Primary Hover**: `#255BB5` — State changes on primary interactive components.
- **Primary Tint**: `#EAF1FC` — Active row selection, low-prominence informational badges, and active tab highlights.

### Neutrals & Surfaces
- **Canvas / Background**: `#F7F8FA` — Default viewport base.
- **Surface Elevation 0**: `#FFFFFF` — Cards, modals, side panels, table rows, and inputs.
- **Border / Divider**: `#E4E7EC` — Crisp 1px division across all surface bounds.
- **Text Primary**: `#101828` — Headings, critical metrics, and table body data.
- **Text Secondary**: `#667085` — Labels, subheadings, and secondary metadata.
- **Text Disabled / Muted**: `#98A2B3` — Inactive controls, placeholder text, and structural icons.

### Severity & Operational Status
Statuses employ precise pairings of dark text, matching 1px borders, and tinted low-saturation fills.
- **Critical (P0)**: Foreground & Border `#D92D20`, Background Tint `#FEF3F2`
- **High / Warning (P1)**: Foreground & Border `#DC6803`, Background Tint `#FFFAEB`
- **Medium / Investigating (P2)**: Foreground & Border `#B54708`, Background Tint `#FFFAEB`
- **Low / Info (P3)**: Foreground & Border `#475467` or `#2E6FDB`, Background Tint `#F2F4F7` or `#EAF1FC`
- **Resolved / Success**: Foreground & Border `#12B76A`, Background Tint `#ECFDF3`

## Typography

The type scale strictly enforces two weights: **400 Regular** for descriptions, long-form content, and contextual metadata; and **600 Semibold** for scannable identifiers, primary headings, tabular numbers, and action labels. 

- **Primary Stack**: `Inter`, -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif. Used for global layout and structure.
- **Monospace Stack**: `JetBrains Mono`, SFMono-Regular, Menlo, Monaco, Consolas, monospace. Strictly utilized for trace IDs, timestamps, commit hashes, payload snippets, IP addresses, and console logs.
- **Numbers**: Render tabular numerals (`font-variant-numeric: tabular-nums`) by default across data tables, telemetry counters, and timer displays to prevent horizontal layout shift during live refreshes.

## Layout & Spacing

Spacing is anchored strictly to an 8px base grid (`8px`, `16px`, `24px`, `32px`, `48px`).

### Master Architecture
- **Navigation Sidebar**: Fixed at `240px` width. Fixed to the viewport edge with a continuous `1px` right border `#E4E7EC`. Collapses to a compact icon-rail at `64px` on tablet viewports (<1024px).
- **Top Utility Bar**: Fixed at `64px` height with a continuous `1px` bottom border `#E4E7EC`. Houses global environment pickers, search command palettes, and global operational indicators.
- **Content Area**: Fluid layout with an upper constraint of `1280px` max-width. Padding on primary surfaces defaults to `24px` (`space-md`).

### Breakpoints & Adaptivity
- **Desktop (>= 1280px)**: Sidebar (240px) + Main Content (max 1280px, centered or pinned left with 24px margins). Multi-column dashboard widgets (2, 3, or 4 equal columns via 16px gutter).
- **Tablet (768px – 1023px)**: Sidebar condenses to 64px icon-only rail. Grid collapses to 1- or 2-column configurations. Gutters compress to 16px, canvas margins to 16px.
- **Mobile (< 768px)**: Sidebar hides behind an explicit off-canvas drawer. All metric cards and incident lists stack linearly into a single column. Horizontal padding defaults to 16px.

## Elevation & Depth

This system intentionally eliminates blurred drop shadows to avoid visual muddying during critical alerts. Hierarchy is established entirely through structural planes, high-contrast borders, and surface contrast:

- **Base Layer (Elevation -1)**: Canvas background `#F7F8FA`.
- **Primary Layer (Elevation 0)**: Panel surfaces, cards, and data tables rendered in `#FFFFFF`, encapsulated by a uniform border: `1px solid #E4E7EC`.
- **Floating Layer (Elevation 1 - Overlays)**: Dropdowns, search palettes, and tooltips retain a `#FFFFFF` fill with `1px solid #E4E7EC` and a minimal structural boundary: `0 4px 12px rgba(16, 24, 40, 0.08)`.
- **Modal Layer (Elevation 2)**: Critical alert dialogs utilize a dark dimming scrim (`rgba(16, 24, 40, 0.4)`) paired with a `#FFFFFF` container and `1px solid #E4E7EC`.

## Shapes

The design language uses a compact, disciplined corner radius (`0.25rem` / `4px`) across functional elements, maintaining precision and visual discipline.

- **Base Components**: Buttons, inputs, search fields, and metric cards use `4px` (`0.25rem`).
- **Containers**: Side sheets, modal dialogs, and outer dashboard cards utilize `rounded-lg` (`8px` / `0.5rem`).
- **Status Pills**: Badges use `4px` corner radiuses rather than circular pills to retain the engineered control-room aesthetic.
- **Dividers**: Strict geometric lines with `0px` radius.

## Components

### Buttons
- **Primary**: Solid `#2E6FDB` background, `#FFFFFF` text, `4px` radius, 36px default height. Hover: `#255BB5`. Focus: 2px ring `#EAF1FC`.
- **Secondary / Outline**: `#FFFFFF` background, `1px solid #E4E7EC`, `#101828` text. Hover: `#F7F8FA` background, border `#D0D5DD`.
- **Destructive**: Solid `#D92D20` background, `#FFFFFF` text. Hover: `#B42318`.
- **Sizes**: Compact (28px height, 11px text), Default (36px height, 13px text), Large (40px height, 14px text).

### Status Badges (Severity Pills)
- Rendered as compact, inline indicators with strict 1px borders and subtle background tints.
- **P0 Critical**: Text `#D92D20`, Border `#D92D20`, Background `#FEF3F2`.
- **P1 High**: Text `#DC6803`, Border `#DC6803`, Background `#FFFAEB`.
- **P2 Medium**: Text `#B54708`, Border `#DC6803`, Background `#FFFAEB`.
- **P3 Low / Info**: Text `#475467`, Border `#E4E7EC`, Background `#F2F4F7`.
- **Resolved**: Text `#12B76A`, Border `#12B76A`, Background `#ECFDF3`.
- Internal structure: 2px vertical / 6px horizontal padding, 11px uppercase bold JetBrains Mono or Inter 600.

### Input Fields & Controls
- **Text Inputs**: Height 36px, `1px solid #E4E7EC`, background `#FFFFFF`, text `#101828`, placeholder `#98A2B3`. Focus state: border `#2E6FDB`, inset outline `1px solid #2E6FDB`.
- **Checkboxes & Radios**: 16x16px square/circle with `1px solid #D0D5DD`. Checked: `#2E6FDB` background with white check or dot.

### Data Tables (Scannable Grid)
- Dense, tabular-first layout. Header height 36px, row height 44px.
- **Header**: `#F7F8FA` background, uppercase `11px` Inter 600, color `#667085`, bottom border `1px solid #E4E7EC`.
- **Rows**: `#FFFFFF` background, bottom border `1px solid #E4E7EC`. Hover: `#F7F8FA`. Selected: `#EAF1FC`.
- Cells containing timestamps, IP addresses, trace IDs, and runtimes use `JetBrains Mono` at `12px`.

### Telemetry & Log Streamers
- Fixed-height console panes with `#101828` background (or high-contrast `#FFFFFF` with `#E4E7EC` border for light-mode log views).
- Monospace output (`code-sm`), selectable text strings, and persistent severity indicators anchored to the left margin.

### Incident Cards
- Encapsulated within `#FFFFFF` with `1px solid #E4E7EC` and `8px` corner radius.
- Includes a prominent header with severity badge, incident ID (`code-sm`), elapsed duration timer with tabular numerals, and responder avatar stack.