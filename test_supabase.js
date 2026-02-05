import { createClient } from '@supabase/supabase-js'

const supabaseUrl = 'https://ucxdhrikpgxgzbdkwzfc.supabase.co'
const supabaseKey = 'sb_publishable_LzMUxGAt_JjwnANm2kupuA_ocSQnIDi'
const supabase = createClient(supabaseUrl, supabaseKey)

async function test() {
    console.log('Testing Supabase Connection...')
    const { data, error } = await supabase.from('Users').select('*').limit(1)
    if (error) {
        console.error('FAILED:', error)
    } else {
        console.log('SUCCESS: Connected to Supabase via REST!', data)
    }
}

test()
