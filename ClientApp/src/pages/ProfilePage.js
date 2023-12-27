import React, { useState, useEffect, useContext } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { Form as AntdForm, message } from 'antd';
import dayjs from 'dayjs';
import {
  Button,
  Form,
  ImageUpload,
  Input,
  InputNumber,
  Label,
  LabelFormItem,
  Loader,
  Modal,
  Table,
  SideBySide,
} from '../ui';
import { 
  BorderBottom,
  ButtonList,
  Color,
  FormHeader,
  StyledPage, 
  StyledWrapper,
} from '../styles/layout/form';
import useQueryApiClient from '../utils/useQueryApiClient';
import { UserDataContext } from '../contexts/UserDataProvider'

export const ProfilePage = () => {
  const [form] = AntdForm.useForm();
  const [addMoneyForm] = AntdForm.useForm();
  const [withdrawMoneyForm] = AntdForm.useForm();

  const [isEditPage, setIsEditPage] = useState(false)
  const [imageUrl, setImageUrl] = useState()

  const [isAddMoneyModalOpened, setIsAddMoneyModalOpened] = useState(false)
  const [isWithdrawMoneyModalOpened, setIsWithdrawMoneyModalOpened] = useState(false)

  const { data: userData, refetch } = useContext(UserDataContext)

  const navigate = useNavigate()
  const location = useLocation();

  useEffect(() => {
    setIsEditPage(location.pathname.endsWith('/edit'))
  }, [location])

  useEffect(() => {
    setImageUrl(userData?.imgSrc)

    form.setFieldsValue({
      imgSrc: userData?.imgSrc,
      firstName: userData?.firstName,
      lastName: userData?.lastName,
    })
  }, [userData])

  const { appendData: updateUser, isLoading: updateUserLoading } = useQueryApiClient({
    request: {
      url: `api/user-data`,
      method: 'PATCH'
    },
    onSuccess: () => {
      message.success('User info is succesfully updated')
      refetch()
      navigate('/profile')
    }
  })

  const { appendData: addMoney, isLoading: addMoneyLoading } = useQueryApiClient({
    request: {
      url: `api/user-data/add-money`,
      method: 'POST'
    },
    onSuccess: () => {
      refetch()
      refetchTransactions()
    }
  })

  const { appendData: withdrawMoney, isLoading: withdrawMoneyLoading } = useQueryApiClient({
    request: {
      url: `api/user-data/withdraw-money`,
      method: 'POST'
    },
    onSuccess: () => {
      refetch()
      refetchTransactions()
    }
  })

  const { data: transactions, refetch: refetchTransactions, isLoading: getTransactionsLoading } = useQueryApiClient({
    request: {
      url: `api/user-data/transactions`,
      method: 'GET'
    }
  })

  const transactionColumns = [
    {
      dataIndex: 'id',
      key: 'id',
      title: 'Id'
    },
    {
      dataIndex: 'created',
      key: 'created',
      title: 'Date',
      render: (date) => dayjs(date).format('DD-MM-YYYY')
    },
    {
      dataIndex: 'amount',
      key: 'amount',
      title: 'Amount',
      render: (amount) => (
        <Color className={amount > 0 ? 'green' : 'red'}>
          {Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'EUR'
          }).format(amount)}
        </Color>
      )
    },
    {
      dataIndex: 'description',
      key: 'description',
      title: 'Details'
    }
  ]

  const goBack = () => {
    setImageUrl(userData?.imgSrc)

    form.setFieldsValue({
      imgSrc: userData?.imgSrc,
      firstName: userData?.firstName,
      lastName: userData?.lastName,
    })

    navigate(-1)
  }

  return (
    <StyledPage>

      <FormHeader>
        <Label 
          label={'Profile'} 
          extraBold 
        />
      </FormHeader>

      <Loader loading={!userData || updateUserLoading} >
        <Form 
          form={form}
          disabled={!isEditPage}
          onFinish={(values) => updateUser(values)}
        >
          <StyledWrapper>
            <Label label={'User info:'} extraBold />

            <BorderBottom />

            <ImageUpload
              form={form}
              imageUrl={imageUrl}
              setImageUrl={setImageUrl}
              name="imgSrc"
              disabled={!isEditPage}
            />

            <BorderBottom />

            <SideBySide
              left={
                <Input
                  name="firstName"
                  label={'First Name'}
                  rules={[{ required: true }]}
                />
              }
            />

            <SideBySide
              left={
                <Input
                  name="lastName"
                  label={'Last Name'}
                  rules={[{ required: true }]}
                />
              }
            />

            <BorderBottom />

            <ButtonList>
              {isEditPage ? (
                <>
                  <Button 
                    htmlType="submit" 
                    type="primary" 
                    label={'Save changes'} 
                    disabled={false}
                  />
                  <Button 
                    onClick={goBack} 
                    label={'Return'}
                    disabled={false}
                  />
                </>
              ) : (
                <Button 
                  type="primary" 
                  onClick={() => navigate('edit')} 
                  label={'Edit'}
                  disabled={false}
                />
              )}
            </ButtonList>

          </StyledWrapper>

        </Form>

        <FormHeader
          style={{
            marginTop: '40px',
            marginBottom: '40px'
          }}
        >
          <Label 
            label={'Balance'} 
            extraBold 
          />

          <LabelFormItem 
            label={'Current balance'} 
            labelValue={userData?.balance}
            currency
          />

          <Button 
            type="primary" 
            onClick={() => setIsAddMoneyModalOpened(true)} 
            label={'Add money'}
            size={'default'}
          />
          <Modal 
            title="Add money" 
            open={isAddMoneyModalOpened} 
            onOk={async () => {
              try {
                const fields = await addMoneyForm.validateFields()
                addMoney(fields)
                
                addMoneyForm.resetFields()
                setIsAddMoneyModalOpened(false)
              } catch (e) {
                console.error(e)
              }
            }} 
            onCancel={() => {
              addMoneyForm.resetFields()
              setIsAddMoneyModalOpened(false)
            }}
          >
            <Form form={addMoneyForm}>
              <InputNumber
                name="amount"
                label={'Amount'}
                min={0}
                precision={2}
                addonBefore={"+"}
                addonAfter={"€"}
                rules={[{ required: true }]}
              />
              <ButtonList className="left small-gap">
                <Button 
                  label={'10€'}
                  size={'small'}
                  onClick={() => addMoneyForm.setFieldValue('amount', (addMoneyForm.getFieldValue('amount') ?? 0) + 10)}
                />
                <Button 
                  label={'50€'}
                  size={'small'}
                  onClick={() => addMoneyForm.setFieldValue('amount', (addMoneyForm.getFieldValue('amount') ?? 0) + 50)}
                />
                <Button 
                  label={'100€'}
                  size={'small'}
                  onClick={() => addMoneyForm.setFieldValue('amount', (addMoneyForm.getFieldValue('amount') ?? 0) + 100)}
                />
                <Button 
                  label={'500€'}
                  size={'small'}
                  onClick={() => addMoneyForm.setFieldValue('amount', (addMoneyForm.getFieldValue('amount') ?? 0) + 500)}
                />
              </ButtonList>
            </Form>
          </Modal>

          <Button 
            type="primary"
            danger
            onClick={() => setIsWithdrawMoneyModalOpened(true)} 
            label={'Withdraw money'}
            size={'default'}
          />
          <Modal 
            title="Withdraw money" 
            open={isWithdrawMoneyModalOpened} 
            onOk={async () => {
              try {
                const fields = await withdrawMoneyForm.validateFields()
                withdrawMoney(fields)

                withdrawMoneyForm.resetFields()
                setIsWithdrawMoneyModalOpened(false)
              } catch (e) {
                console.error(e)
              }
            }} 
            onCancel={() => {
              withdrawMoneyForm.resetFields()
              setIsWithdrawMoneyModalOpened(false)
            }}
          >
            <Form form={withdrawMoneyForm}>
              <InputNumber
                name="amount"
                label={'Amount'}
                min={0}
                precision={2}
                addonBefore={"-"}
                addonAfter={"€"}
                rules={[{ required: true }]}
              />
              <ButtonList>
                <Button 
                  label={'10€'}
                  size={'small'}
                  onClick={() => withdrawMoneyForm.setFieldValue('amount', (withdrawMoneyForm.getFieldValue('amount') ?? 0) + 10)}
                />
                <Button 
                  label={'50€'}
                  size={'small'}
                  onClick={() => withdrawMoneyForm.setFieldValue('amount', (withdrawMoneyForm.getFieldValue('amount') ?? 0) + 50)}
                />
                <Button 
                  label={'100€'}
                  size={'small'}
                  onClick={() => withdrawMoneyForm.setFieldValue('amount', (withdrawMoneyForm.getFieldValue('amount') ?? 0) + 100)}
                />
                <Button 
                  label={'500€'}
                  size={'small'}
                  onClick={() => withdrawMoneyForm.setFieldValue('amount', (withdrawMoneyForm.getFieldValue('amount') ?? 0) + 500)}
                />
              </ButtonList>
            </Form>
          </Modal>


        </FormHeader>
      </Loader>

      <StyledWrapper>
        <Label label={'Transactions'} extraBold />

        <BorderBottom />

        <Table 
          dataSource={transactions?.data}
          columns={transactionColumns}
          loading={getTransactionsLoading || addMoneyLoading || withdrawMoneyLoading}
        />
      </StyledWrapper>
    </StyledPage>
  )
}
