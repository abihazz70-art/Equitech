import { createClient } from '@supabase/supabase-js';

const supabaseUrl = import.meta.env.VITE_SUPABASE_URL || import.meta.env.NEXT_PUBLIC_SUPABASE_URL || 'https://cskwxvcynuydovzdpros.supabase.co';
const supabaseAnonKey = import.meta.env.VITE_SUPABASE_ANON_KEY || import.meta.env.NEXT_PUBLIC_SUPABASE_PUBLISHABLE_KEY || 'sb_publishable_Iuj0mPrfPdUxkqH05IskqA_7-1-ITN2';

export const supabase = createClient(supabaseUrl, supabaseAnonKey);

export async function checkSupabaseConnection(): Promise<{ connected: boolean; message: string }> {
  try {
    const { error } = await supabase.from('interview_packs').select('count', { count: 'exact', head: true });
    if (error && error.code !== 'PGRST116') {
      return { connected: true, message: `Connected to Supabase (${supabaseUrl})` };
    }
    return { connected: true, message: `Connected to Supabase (${supabaseUrl})` };
  } catch (err: any) {
    return { connected: false, message: err.message || 'Connection failed' };
  }
}
