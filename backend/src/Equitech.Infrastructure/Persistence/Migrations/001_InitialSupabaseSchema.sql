-- ==============================================================================
-- Equitech Job Search & Career Development Agent: Supabase Initial Migration
-- Run this script in your Supabase SQL Editor (https://supabase.com/dashboard/project/cskwxvcynuydovzdpros/sql/new)
-- ==============================================================================

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- 1. Interview Packs Table (Appendix A5 Output Contract)
CREATE TABLE IF NOT EXISTS public.interview_packs (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL DEFAULT '00000000-0000-0000-0000-000000000001',
    job_id VARCHAR(100) NOT NULL,
    profile_id VARCHAR(100) NOT NULL,
    version INT NOT NULL DEFAULT 1,
    content_json JSONB NOT NULL DEFAULT '{}'::jsonb,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_interview_packs_user_job ON public.interview_packs(user_id, job_id);

-- 2. Practice Sessions Table (Mock Interview / Practical Exercise Submissions)
CREATE TABLE IF NOT EXISTS public.practice_sessions (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL DEFAULT '00000000-0000-0000-0000-000000000001',
    interview_pack_id UUID REFERENCES public.interview_packs(id) ON DELETE CASCADE,
    started_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    completed_at TIMESTAMPTZ,
    answers_json JSONB NOT NULL DEFAULT '{}'::jsonb,
    score_json JSONB,
    feedback_json JSONB
);

-- 3. Learning Plans Table
CREATE TABLE IF NOT EXISTS public.learning_plans (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL DEFAULT '00000000-0000-0000-0000-000000000001',
    target_role VARCHAR(200) NOT NULL,
    job_id VARCHAR(100),
    status VARCHAR(50) NOT NULL DEFAULT 'Active',
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- 4. Enable Row Level Security (RLS) with Permissive Development Access
ALTER TABLE public.interview_packs ENABLE ROW LEVEL SECURITY;
DROP POLICY IF EXISTS "Allow anon all on interview_packs" ON public.interview_packs;
CREATE POLICY "Allow anon all on interview_packs" ON public.interview_packs FOR ALL USING (true) WITH CHECK (true);

ALTER TABLE public.practice_sessions ENABLE ROW LEVEL SECURITY;
DROP POLICY IF EXISTS "Allow anon all on practice_sessions" ON public.practice_sessions;
CREATE POLICY "Allow anon all on practice_sessions" ON public.practice_sessions FOR ALL USING (true) WITH CHECK (true);

ALTER TABLE public.learning_plans ENABLE ROW LEVEL SECURITY;
DROP POLICY IF EXISTS "Allow anon all on learning_plans" ON public.learning_plans;
CREATE POLICY "Allow anon all on learning_plans" ON public.learning_plans FOR ALL USING (true) WITH CHECK (true);
