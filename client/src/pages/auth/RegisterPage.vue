<template>
  <q-page class="flex flex-center bg-dark text-white q-py-lg q-pb-xl overflow-hidden relative-position">
     <!-- Dynamic Background -->
     <div class="absolute-full" style="background: radial-gradient(circle at 50% 120%, #1e3a8a 0%, #000 60%); z-index: 0;"></div>

    <q-card
      class="glass-card q-pa-lg z-10"
      style="width: 100%; max-width: 500px; margin-top: 100px"
    >
      <q-card-section class="text-center">
        <div class="text-h4 text-weight-bold q-mb-sm">Identity Verification</div>
        <div class="text-grey-4">Create your secure CBDC account</div>
      </q-card-section>

      <q-card-section>
        <q-form @submit="onSubmit" class="q-gutter-md">
          <!-- Step 1: Personal Info -->
          <div class="text-subtitle2 text-primary text-uppercase letter-spacing-1">
            1. Personal Information
          </div>

          <div class="row q-col-gutter-sm">
            <div class="col-6">
              <q-input
                dark
                filled
                v-model="firstName"
                class="input-box"
                label="First Name"
                :rules="[val => !!val || 'Required']"
              />
            </div>
            <div class="col-6">
              <q-input
                dark
                filled
                v-model="lastName"
                class="input-box"
                label="Last Name"
                :rules="[val => !!val || 'Required']"
              />
            </div>
          </div>

          <q-input
            dark
            filled
            v-model="nationalId"
            class="input-box"
            label="National Identity Number (NID)"
            :rules="[val => !!val || 'Required']"
          >
            <template v-slot:prepend>
              <q-icon name="fingerprint" class="text-primary" />
            </template>
          </q-input>

          <!-- Step 2: Credentials -->
          <div class="text-subtitle2 text-primary text-uppercase letter-spacing-1 q-mt-lg">
            2. Security Credentials
          </div>

          <q-input
            dark
            filled
            v-model="email"
            class="input-box"
            type="email"
            label="Email Address"
            :rules="[val => !!val || 'Required', val => /.+@.+\..+/.test(val) || 'Invalid email']"
          >
            <template v-slot:prepend>
              <q-icon name="email" />
            </template>
          </q-input>

          <q-input
            dark
            filled
            v-model="password"
            class="input-box"
            type="password"
            label="Create Password"
            :rules="[val => !!val || 'Required', val => val.length >= 6 || 'Min 6 characters']"
          >
            <template v-slot:prepend>
              <q-icon name="lock" />
            </template>
          </q-input>

          <q-checkbox
            dark
            v-model="terms"
            label="I agree to the Terms of Sovereign Service"
            size="sm"
            :rules="[val => !!val || 'You must agree']"
          />

          <div class="q-mt-lg q-mb-md">
            <q-btn
              label="Verify & Create Wallet"
              type="submit"
              color="primary"
              class="full-width text-weight-bold q-py-sm shadow-glow-sm"
              rounded
              unelevated
              icon-right="verified_user"
              :loading="loading"
            />
          </div>
        </q-form>
      </q-card-section>

      <q-card-section class="text-center q-pt-none">
        <div class="text-grey-4 text-caption">
          Already verified?
          <router-link
            to="/auth/login"
            class="text-primary text-weight-bold"
            style="text-decoration: none"
            >Log In</router-link
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
import { registerUser } from 'src/api'

const router = useRouter()
const $q = useQuasar()

const firstName = ref('')
const lastName = ref('')
const nationalId = ref('')
const email = ref('')
const password = ref('')
const terms = ref(false)
const loading = ref(false)

async function onSubmit() {
  loading.value = true
  try {
    const data = await registerUser({
      email: email.value,
      password: password.value,
      firstName: firstName.value,
      lastName: lastName.value,
      nationalId: nationalId.value
    })

    if (data.success) {
      localStorage.setItem('cbdc_user', JSON.stringify(data.user))
       $q.notify({
        color: 'green-4',
        textColor: 'white',
        icon: 'check_circle',
        message: 'Wallet Identity Verified! Redirecting...',
        timeout: 2000,
      })
      setTimeout(() => router.push('/dashboard'), 1500)
    } else {
      throw new Error(data.message || 'Registration failed')
    }
  } catch (err) {
    $q.notify({
      color: 'red-5',
      textColor: 'white',
      icon: 'error',
      message: err.message,
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
.letter-spacing-1 {
  letter-spacing: 1px;
}
.shadow-glow-sm {
  box-shadow: 0 0 15px rgba(0, 210, 255, 0.3);
}
.z-10 { z-index: 10; }
</style>
