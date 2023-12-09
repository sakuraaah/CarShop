import React from 'react';
import { Icon } from '../icon';
import { Drawer as AntdDrawer } from 'antd';

export const Drawer = (props) => {
  const app = document.getElementById('root') // TODO check

  return (
    <AntdDrawer
      {...props}
      // closeIcon={<Icon icon="times" faBase="far" />}
      getContainer={() => app}
    />
  );
};
