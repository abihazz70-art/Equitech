import type { JobDescription, CandidateProfile } from '../types';

export const sampleJobs: JobDescription[] = [
  {
    source_id: 'src_internal_001',
    source_job_id: 'JD-DA-2026-01',
    title: 'Data Analyst - Merchant Analytics',
    employer: 'PaySphere Technologies Ltd.',
    canonical_url: 'https://careers.paysphere.example.com/jobs/da-01',
    location: 'Mumbai, India (Hybrid)',
    country_code: 'IND',
    employment_type: 'Full-time',
    salary_min: 700000,
    salary_max: 1000000,
    salary_currency: 'INR',
    posted_at: '2026-10-15T09:00:00Z',
    status: 'Verified open',
    description: `PaySphere is seeking a Data Analyst to join our Merchant Analytics team. You will analyze high-volume payment transaction datasets to identify drop-off trends, build automated KPI dashboards, and support product decision-making.

Key Responsibilities:
- Write advanced SQL queries (CTEs, Window Functions) on Snowflake/PostgreSQL databases.
- Design and maintain interactive Power BI dashboards tracking merchant settlement cycles, churn, and transaction success rates.
- Perform exploratory data analysis using Python (Pandas, NumPy) and basic statistical testing.
- Collaborate with product managers to formulate A/B test hypotheses and interpret sample size experiments.
- Build lightweight ETL data transformations using SQL or dbt.

Required Qualifications:
- 2+ years of professional experience in data analysis or business intelligence.
- Mastery of SQL including query optimization and multi-table joins.
- Demonstrated experience building production dashboards in Power BI.
- Working knowledge of Python for data manipulation.
- Strong stakeholder communication skills.`
  },
  {
    source_id: 'src_internal_002',
    source_job_id: 'JD-EQ-2026-02',
    title: 'Equity Research Associate - Banking & Financial Services',
    employer: 'Apex Global Securities Ltd.',
    canonical_url: 'https://careers.apexsecurities.example.com/jobs/eq-02',
    location: 'Mumbai, India (Onsite)',
    country_code: 'IND',
    employment_type: 'Full-time',
    salary_min: 1200000,
    salary_max: 1800000,
    salary_currency: 'INR',
    posted_at: '2026-10-14T08:30:00Z',
    status: 'Verified open',
    description: `Apex Global Securities is hiring an Equity Research Associate to support our Lead Banking Analyst covering large-cap Indian commercial banks and NBFCs.

Key Responsibilities:
- Construct comprehensive 3-statement financial models and dynamic forecasting models for covered commercial banks and NBFCs.
- Conduct discounted cash flow (DCF), Dividend Discount Model (DDM), and residual income valuations.
- Draft thematic initiation-of-coverage reports, quarterly earnings reviews, and management conference call takeaway notes.
- Track high-frequency banking metrics: Net Interest Margins (NIM), NPA slippages, Provision Coverage Ratio (PCR), and Capital Adequacy (CRAR).
- Support institutional client queries and prepare presentation decks for fund managers.

Required Qualifications:
- MBA in Finance, Chartered Accountant (CA), or CFA charterholder / Level 2+ candidate.
- Minimum 2 years of prior experience in equity research, investment banking, or corporate credit evaluation.
- Demonstrated financial modeling capabilities with strong attention to accounting disclosures.
- In-depth understanding of RBI regulations and Indian banking balance sheet dynamics.`
  },
  {
    source_id: 'src_internal_003',
    source_job_id: 'JD-SWE-2026-03',
    title: 'Software Developer - Full Stack (.NET 8 & Vue 3)',
    employer: 'Novus Cloud Software',
    canonical_url: 'https://careers.novuscloud.example.com/jobs/swe-03',
    location: 'Bengaluru, India (Remote)',
    country_code: 'IND',
    employment_type: 'Full-time',
    salary_min: 900000,
    salary_max: 1400000,
    salary_currency: 'INR',
    posted_at: '2026-10-16T07:15:00Z',
    status: 'Verified open',
    description: `Novus Cloud Software builds enterprise workflow automation platforms. We are looking for a Full Stack Software Developer to design and implement resilient REST APIs and reactive modern web UIs.

Key Responsibilities:
- Develop high-performance backend microservices using ASP.NET Core Web API (.NET 8) and Entity Framework Core.
- Build single-page application (SPA) frontends using Vue 3 (Composition API), TypeScript, and Pinia.
- Design relational schemas, write optimized SQL queries, and manage migrations in PostgreSQL.
- Implement secure authentication flows utilizing OAuth2 / OpenID Connect (OIDC) and JWT.
- Containerize applications using Docker and write automated unit/integration tests with xUnit and Vitest.

Required Qualifications:
- 2-4 years of software development experience with C# / .NET.
- Hands-on experience developing RESTful APIs and working with relational databases (PostgreSQL or SQL Server).
- Frontend proficiency with modern JavaScript/TypeScript and a modern reactive framework (Vue, React, or Angular).
- Solid understanding of OOP principles, Clean Architecture, and dependency injection.`
  }
];

export const sampleCandidates: CandidateProfile[] = [
  {
    profile_id: 'cand_001_aarav',
    full_name: 'Aarav Sharma',
    headline: 'Junior BI Analyst | SQL & Excel Specialist',
    location: 'Pune, India',
    summary: 'Data analyst with 1.5 years of experience in retail reporting, building Excel financial summaries and SQL database extractions. Seeking to specialize in FinTech product analytics.',
    employment_history: [
      {
        company: 'Kalyan Retail Solutions',
        role: 'Junior Reporting Analyst',
        start_date: '2025-05-01',
        end_date: null,
        is_current: true,
        responsibilities: [
          'Maintained weekly sales reporting across 45 stores using MS Excel (VLOOKUP, Pivot Tables, Power Query).',
          'Wrote basic SQL queries on PostgreSQL to extract daily inventory and stock movement logs.',
          'Created an experimental Power BI report for regional store performance.'
        ]
      }
    ],
    education: [
      { institution: 'Pune University', degree: 'Bachelor of Science in Statistics', graduation_year: 2024 }
    ],
    skills: [
      { name: 'SQL (Basic Joins, Group By)', proficiency: 'Intermediate', evidence: 'Extracted daily inventory logs at Kalyan Retail.' },
      { name: 'Microsoft Excel (VLOOKUP, Power Query)', proficiency: 'Advanced', evidence: 'Automated regional sales reports.' },
      { name: 'Power BI', proficiency: 'Beginner', evidence: 'Created 1 pilot store report.' },
      { name: 'Descriptive Statistics', proficiency: 'Intermediate', evidence: 'B.Sc. in Statistics coursework.' }
    ],
    certifications: [],
    achievements: [
      'Reduced weekly sales report turnaround time from 6 hours to 2 hours via Power Query automation.'
    ]
  },
  {
    profile_id: 'cand_002_meera',
    full_name: 'Meera Iyer',
    headline: 'Credit Risk Analyst | CA Finalist | Financial Modeling',
    location: 'Mumbai, India',
    summary: 'Chartered Accountant with 2.5 years of corporate credit risk assessment experience in commercial banking. Skilled in statutory audit, cash flow analysis, and balance sheet stress testing. Transitioning to sell-side equity research.',
    employment_history: [
      {
        company: 'Vistara Commercial Bank',
        role: 'Credit Analyst - Corporate Banking',
        start_date: '2024-03-01',
        end_date: null,
        is_current: true,
        responsibilities: [
          'Evaluated working capital and term loan proposals for mid-market manufacturing companies.',
          'Analyzed audited financial statements, CMA reports, debt service coverage ratios (DSCR), and collateral adequacy.',
          'Prepared credit approval memorandums presented to the Credit Committee.'
        ]
      },
      {
        company: 'R.K. Joshi & Associates, CAs',
        role: 'Articled Assistant',
        start_date: '2021-01-15',
        end_date: '2024-01-15',
        is_current: false,
        responsibilities: [
          'Conducted statutory and internal audits of public and private sector banks, focusing on NPA identification and provisioning compliance under RBI guidelines.'
        ]
      }
    ],
    education: [
      { institution: 'ICAI', degree: 'Chartered Accountant (CA)', graduation_year: 2024 },
      { institution: 'CFA Institute', degree: 'CFA Level 1 Passed', graduation_year: 2025 }
    ],
    skills: [
      { name: 'Financial Statement Analysis', proficiency: 'Advanced', evidence: 'Underwrote 40+ corporate credit loan proposals.' },
      { name: 'RBI Banking Regulations & NPA Provisioning', proficiency: 'Advanced', evidence: 'Bank statutory audits at R.K. Joshi.' },
      { name: 'Credit Risk & DSCR Modeling', proficiency: 'Intermediate', evidence: 'Built corporate borrower cash flow stress models.' },
      { name: 'Financial Modeling in Excel', proficiency: 'Intermediate', evidence: 'Constructed corporate debt repayment schedules.' }
    ],
    certifications: ['Chartered Accountant (ICAI, 2024)'],
    achievements: ['Ranked in top 5% of ICAI CA Final Examination (Nov 2023).']
  },
  {
    profile_id: 'cand_003_rohan',
    full_name: 'Rohan Deshmukh',
    headline: 'Backend C# .NET Developer | ASP.NET Core & SQL Server',
    location: 'Bengaluru, India',
    summary: 'Backend software engineer with 2.5 years of experience building C# REST APIs and enterprise SQL Server databases. Strong in clean code, asynchronous programming, and unit testing. Actively expanding frontend skills in Vue 3.',
    employment_history: [
      {
        company: 'Trident Tech Enterprises',
        role: 'Software Engineer',
        start_date: '2024-06-01',
        end_date: null,
        is_current: true,
        responsibilities: [
          'Developed and maintained 12+ RESTful endpoints in ASP.NET Core 7.0 for order processing services.',
          'Implemented Entity Framework Core code-first migrations and wrote stored procedures in MS SQL Server.',
          'Authored unit and integration test suites using xUnit and Moq, achieving 82% test coverage.'
        ]
      }
    ],
    education: [
      { institution: 'Visvesvaraya Technological University (VTU)', degree: 'B.E. in Computer Science', graduation_year: 2023 }
    ],
    skills: [
      { name: 'C# / ASP.NET Core', proficiency: 'Intermediate', evidence: '2.5 years production API development at Trident Tech.' },
      { name: 'Entity Framework Core & SQL Server', proficiency: 'Intermediate', evidence: 'Built relational schemas and LINQ queries.' },
      { name: 'xUnit & Moq Testing', proficiency: 'Intermediate', evidence: 'Maintained 82% code coverage.' },
      { name: 'JavaScript / HTML / CSS', proficiency: 'Beginner', evidence: 'Maintained internal jQuery tooling.' },
      { name: 'Git & GitHub', proficiency: 'Intermediate', evidence: 'Daily collaborative branching workflows.' }
    ],
    certifications: ['Microsoft Certified: Azure Fundamentals (AZ-900)'],
    achievements: [
      'Optimized order processing API endpoint latency by 40% using asynchronous tasks and EF Core eager loading.'
    ]
  }
];
