<template>
  <q-page class="flex flex-center bg-dark text-white q-pb-xl overflow-hidden relative-position">
     <!-- Dynamic Background -->
     <div class="absolute-full" style="background: radial-gradient(circle at 50% 120%, #1e3a8a 0%, #000 60%); z-index: 0;"></div>

    <q-card
      class="glass-card q-pa-lg z-10"
      style="width: 100%; max-width: 400px; margin-top: 100px"
    >
      <q-card-section class="text-center">
        <div class="text-h4 text-weight-bold q-mb-sm">Welcome Back</div>
        <div class="text-grey-4">Sign in to your sovereign wallet</div>
      </q-card-section>

      <q-card-section>
        <q-form @submit="onSubmit" class="q-gutter-md">
          <q-input
            dark
            filled
            v-model="email"
            class="input-box"
            label="Email Address"
            lazy-rules
            :rules="[(val) => (val && val.length > 0) || 'Please enter your email']"
          >
            <template v-slot:prepend>
              <q-icon name="badge" class="text-primary" />
            </template>
          </q-input>

          <q-input
            dark
            filled
            type="password"
            v-model="password"
            class="input-box"
            label="Password"
            lazy-rules
            :rules="[(val) => (val && val.length > 0) || 'Please enter your password']"
          >
            <template v-slot:prepend>
              <q-icon name="lock" class="text-primary" />
            </template>
          </q-input>

          <div class="row justify-between items-center">
            <q-checkbox dark v-model="rememberMe" label="Remember me" size="sm" />
            <a
              href="#"
              class="text-primary text-caption text-weight-bold"
              style="text-decoration: none"
              >Forgot Password?</a
            >
          </div>

          <div class="q-mb-md">
            <q-btn
              label="Secure Login"
              type="submit"
              color="primary"
              class="full-width text-weight-bold q-py-sm shadow-glow-sm"
              rounded
              unelevated
              icon-right="login"
              :loading="loading"
            />
          </div>
        </q-form>
      </q-card-section>

      <q-card-section class="text-center q-pt-none">
        <div class="text-grey-4 text-caption">
          Don't have a wallet?
          <router-link
            to="/auth/register"
            class="text-secondary text-weight-bold"
            style="text-decoration: none"
            >Verify Identity</router-link
          >
        </div>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useQuasar } from 'quasar'
import { loginUser } from 'src/api'

const router = useRouter()
const $q = useQuasar()
const email = ref('')
const password = ref('')
const rememberMe = ref(false)
const loading = ref(false)

async function onSubmit() {
  loading.value = true
  try {
    const response = await loginUser({
      email: email.value,
      password: password.value
    })

    if (response.success) {
      $q.notify({
        color: 'green-4',
        textColor: 'white',
        icon: 'check_circle',
        message: 'Login Successful! Welcome back.',
      })
      
      // Store user data in localStorage
      localStorage.setItem('cbdc_user', JSON.stringify(response.user))
      
      router.push('/dashboard')
    } else {
      $q.notify({
        color: 'red-5',
        textColor: 'white',
        icon: 'error',
        message: response.message || 'Login failed. Please check your credentials.',
      })
    }
  } catch (error) {
    console.error('Login error:', error)
    $q.notify({
      color: 'red-8',
      textColor: 'white',
      icon: 'report_problem',
      message: 'Network error. Please ensure the backend is running and CORS is configured.',
    })
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.glass-card {
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 16px;
}
.shadow-glow-sm {
  box-shadow: 0 0 15px rgba(0, 210, 255, 0.3);
}
.z-10 { z-index: 10; }
</style>
